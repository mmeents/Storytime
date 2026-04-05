using Azure.Core;
using KB.Core.Entities;
using Storytime.Core.Entities;
using KB.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Storytime.Core.Agents;
using Storytime.Core.Handlers.Export;
using Storytime.Core.Handlers.Items;
using Storytime.Core.Models;
using Storytime.Core.Handlers.Queue;



namespace Storytime.Core.Service {
  public interface IAppDataModuleService {
    AgentRunnerMode CurrentMode { get; set; }
    string CurrentLMStudioModel { get; set; }
    string CurrentClaudeModel { get; set; }
    Task<List<ItemDto>> GetAllProjectItems();
    Task<ItemDto?> GetItemById(int? id);
    Task<List<ItemRelationDto>> GetAllRelations();
    Task<List<ItemTypeDto>> GetAllItemTypes();
    Task<List<ItemRelationTypeDto>> GetAllRelationTypes();
    Task<ItemRelationDto?> UpdateItemRelation(ItemRelationDto relationDto);
    Task<ItemRelationDto?> CreateItemRelation(ItemRelationDto relationDto);
    Task<bool> DeleteItem(int itemId);
    Task<ItemDto?> UpdateItem(ItemDto itemDto);
    Task<ItemDto?> CreateItem(ItemDto itemDto);
    Task<ItemRelationDto?> CreateRelation(int itemId, int? relatedItemId, int relationTypeId);

    Task<List<string>> GetLmStudioModels();
    Task<bool> GenerateStory(int projectId);
    Task<bool> GenerateSceneAndCharacterForStory(int storyId);
    Task<bool> GenerateBeatsForScene(int storyId, int sceneId);
    Task<bool> GenerateCallSheetForStoryScene(int storyId, int sceneId);
    Task<bool> GeneratePerformanceForCallSheet( int storyId, int callSheetId);
    Task<bool> GenerateDeliverableForPerformance(int performanceId);
    Task<ExportItemCommandResult> ExportItem(int itemId, bool exportChildren, string exportPath);

    Task<AgentQueueItem?> AddToSchedule(int itemId, StItemType targetDepth);
    Task<AgentQueueItem?> GetAgentQueueItemById(int id);
    Task<AgentQueueItem?> UpateAgentQueueItemStatusCommand(int id, AgentQueueStatus status, string? errorMessage = null);    
    Task<List<AgentQueueItem>> GetAgentQueueQuery();

  }

  public class AppDataModuleService : IAppDataModuleService {    
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IFactorySettingsService _factorySettingsService; 

    public AppDataModuleService(IServiceScopeFactory scopeFactory, IFactorySettingsService factorySettingsService) { 
        _scopeFactory = scopeFactory;        
        _factorySettingsService = factorySettingsService;
    }

    public AgentRunnerMode CurrentMode { 
      get {
        return _factorySettingsService.CurrentMode;            
      }
      set {
        _factorySettingsService.CurrentMode = value;
     
      }
    }

    public string CurrentLMStudioModel { 
      get {
        return _factorySettingsService.LMStudioModel;            
      }
      set {
        _factorySettingsService.LMStudioModel = value;
      }
    }

    public string CurrentClaudeModel { 
      get {
        return _factorySettingsService.ClaudeModel;            
      }
      set {
        _factorySettingsService.ClaudeModel = value;
      }
    }

    public async Task<List<ItemDto>> GetAllProjectItems() {
      var context = GetDbContext();

      var query = context.Items        
        .Where(i => i.ItemTypeId == (int)StItemType.Project && i.IsActive 
          && !i.IncomingRelations.Any(r => r.Item.ItemTypeId == (int)StItemType.Project))
        .Include(i => i.ItemType)
            .Include(i => i.Relations)
                .ThenInclude(r => r.RelatedItem)
            .Include(i => i.Relations)
                .ThenInclude(r => r.RelationType)
            .Include(i => i.IncomingRelations)
                .ThenInclude(r => r.Item)
            .Include(i => i.IncomingRelations)
                .ThenInclude(r => r.RelationType)
        .OrderBy(i => i.Name).Select(i => i.ToDto(true));      


      var resultList = await query.ToListAsync();      
      return resultList;
    }

    public async Task<ItemDto?> GetItemById(int? id) {
      if (id == null) return null;
      var context = GetDbContext();
      var query = context.Items
          .AsNoTracking()
          .Where(i => i.Id == id && i.IsActive)
          .Include(i => i.ItemType)
          .Include(i => i.Relations)
              .ThenInclude(r => r.RelatedItem)
          .Include(i => i.Relations)
              .ThenInclude(r => r.RelationType)
          .Include(i => i.IncomingRelations)
              .ThenInclude(r => r.Item)
          .Include(i => i.IncomingRelations)
              .ThenInclude(r => r.RelationType);
      var item = await query.FirstOrDefaultAsync();
      return item != null ? item.ToDto(true) : null;
    }

    public async Task<List<ItemRelationDto>> GetAllRelations() {
      var context = GetDbContext();
      var query = context.ItemRelations
        .Include(ir => ir.Item)
        .Include(ir => ir.RelatedItem)
        .Include(ir => ir.RelationType)
        .AsNoTracking()
        .AsQueryable();

      query = query.OrderByDescending(ir => ir.Id);
      var result = await query.Select(ir => ir.ToDto()).ToListAsync();
      return result;
    }

    public async Task<List<ItemTypeDto>> GetAllItemTypes() {
      var context = GetDbContext();
      var query = context.ItemTypes.AsNoTracking().AsQueryable();
      var result = await query.OrderBy(it => it.Name).Select(it => it.ToDto()).ToListAsync();
      return result;
    }

    public async Task<List<ItemRelationTypeDto>> GetAllRelationTypes() {
      var context = GetDbContext();
      var query = context.ItemRelationTypes.AsNoTracking().AsQueryable();
      var result = await query.OrderBy(irt => irt.Relation).Select(irt => irt.ToDto()).ToListAsync();
      return result;
    }

    public async Task<ItemRelationDto?> UpdateItemRelation(ItemRelationDto relationDto) {
      var context = GetDbContext();
      var relation = await context.ItemRelations.FindAsync(relationDto.Id);
      if (relation == null) {
        return null;
      }
      relation.ItemId = relationDto.ItemId;
      relation.RelatedItemId = relationDto.RelatedItemId;
      relation.RelationTypeId = relationDto.RelationTypeId;
      relation.Rank = relationDto.Rank;
      await context.SaveChangesAsync();
      return relation.ToDto();
    }

    public async Task<ItemRelationDto?> CreateItemRelation(ItemRelationDto relationDto) {
      var context = GetDbContext();
      var nextRank = await context.ItemRelations
        .Where(ir => ir.ItemId == relationDto.ItemId)
        .CountAsync() + 1;
      var relation = new ItemRelation {
        ItemId = relationDto.ItemId,
        RelatedItemId = relationDto.RelatedItemId,
        RelationTypeId = relationDto.RelationTypeId,
        Rank = nextRank
      };
      context.ItemRelations.Add(relation);
      await context.SaveChangesAsync();
      return relation.ToDto();
    }
    public async Task<bool> DeleteItem(int itemId) {
      using var scope = _scopeFactory.CreateScope();
      var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();      
      var command = new DeleteItemCommand(itemId);
      return await mediator.Send(command);
    }
    public async Task<ItemDto?> UpdateItem(ItemDto itemDto) {
      var context = GetDbContext();
      var item = await context.Items.FindAsync(itemDto.Id);
      if (item == null) {
        return null;
      }
      item.Name = itemDto.Name;
      item.Description = itemDto.Description;
      item.Data = itemDto.Data;
      item.ItemTypeId = itemDto.ItemTypeId;
      item.IsActive = itemDto.IsActive;
      await context.SaveChangesAsync();
      return item.ToDto();
    }

    public async Task<ItemDto?> CreateItem(ItemDto itemDto) {
      var context = GetDbContext();
      var item = new Item {
        Name = itemDto.Name,
        Description = itemDto.Description,
        Data = itemDto.Data,
        ItemTypeId = itemDto.ItemTypeId,
        IsActive = true
      };
      context.Items.Add(item);
      await context.SaveChangesAsync();
      return item.ToDto();
    }

    public async Task<ItemRelationDto?> CreateRelation(int itemId, int? relatedItemId, int relationTypeId) {
      var context = GetDbContext();
      var item = await context.Items.FindAsync(itemId);
      var relatedItem = relatedItemId.HasValue ? await context.Items.FindAsync(relatedItemId.Value) : null;

      var relationType = await context.ItemRelationTypes.FindAsync(relationTypeId);
      if (item == null || relationType == null) {
        return null;
      }
      var nextRank = await context.ItemRelations
        .Where(ir => ir.ItemId == itemId)
        .CountAsync() + 1;
      var relation = new ItemRelation {
        ItemId = itemId,
        RelatedItemId = relatedItemId,
        RelationTypeId = relationTypeId,
        Rank = nextRank
      };
      context.ItemRelations.Add(relation);
      await context.SaveChangesAsync();
      return relation.ToDto();
    }

    public async Task<List<string>> GetLmStudioModels() { 
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var query = new Handlers.LmStudio.GetModelsQuery();
        var result = await mediator.Send(query);
        return result;
    }

    public async Task<bool> GenerateStory(int projectId) {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var command = new Handlers.LmStudio.GenerateStoryCommand(projectId);
        var result = await mediator.Send(command);
        return result;
    }
    
    public async Task<bool> GenerateSceneAndCharacterForStory(int storyId) {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var command = new Handlers.LmStudio.GenerateSceneAndCharacterForStoryCommand(storyId);
        var result = await mediator.Send(command);
        return result;
    }

    public async Task<bool> GenerateBeatsForScene(int storyId, int sceneId) {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var command = new Handlers.LmStudio.GenerateBeatsForSceneCommand(storyId, sceneId);
        var result = await mediator.Send(command);
        return result;
    }

    public async Task<bool> GenerateCallSheetForStoryScene(int storyId, int sceneId) {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        var command = new Handlers.LmStudio.GenerateCallSheetCommand(storyId, sceneId);
        var result = await mediator.Send(command);
        return result;
    }

    public async Task<bool> GeneratePerformanceForCallSheet(int storyId, int callSheetId) {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        var command = new Handlers.LmStudio.GeneratePerformanceForCallSheetCommand(storyId, callSheetId);
        var result = await mediator.Send(command);
        return result;
    }

    public async Task<bool> GenerateDeliverableForPerformance(int performanceId) {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();        
        var command = new Handlers.LmStudio.GenerateDeliverableCommand(performanceId);
        var result = await mediator.Send(command);
        return result;
    }

    public async Task<ExportItemCommandResult> ExportItem(int itemId, bool exportChildren, string exportPath) {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();        
        var command = new ExportItemCommand(itemId, exportChildren, exportPath);
        var result = await mediator.Send(command);
        return result;
    }

    public async Task<AgentQueueItem?> AddToSchedule(int itemId, StItemType targetDepth) {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();        
        var command = new AddAgentQueueItemCommand(itemId, targetDepth);
            var result = await mediator.Send(command);
            return result;
    }

    public async Task<AgentQueueItem?> UpateAgentQueueItemStatusCommand(int id, AgentQueueStatus status, string? errorMessage = null) {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var command = new UpdateAgentQueueItemStatusCommand(id, status, errorMessage);
        var result = await mediator.Send(command);
        return result;
    }
    public async Task<List<AgentQueueItem>> GetAgentQueueQuery() {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();        
        var query = new GetAgentQueueQuery(AgentQueueStatus.Pending);
        var result = await mediator.Send(query);
        return result;
    }

    public async Task<AgentQueueItem?> GetAgentQueueItemById(int id) { 
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var query = new GetAgentQueueItemById(id);
        var result = await mediator.Send(query);
        return result;
    }


    private StorytimeDbContext GetDbContext() {
      var scope = _scopeFactory.CreateScope();
      return scope.ServiceProvider.GetRequiredService<StorytimeDbContext>();
    }
  }
}
