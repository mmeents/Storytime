using KB.Core.Models;
using KB.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;


namespace Storytime.Core.Service {
  public interface IAppDataModuleService {
    Task<List<ItemDto>> GetAllProjectItems();
    Task<ItemDto?> GetItemById(int? id);
    Task<List<ItemRelationDto>> GetAllRelations();
    Task<List<ItemTypeDto>> GetAllItemTypes();
    Task<List<ItemRelationTypeDto>> GetAllRelationTypes();
    Task<ItemRelationDto?> UpdateItemRelation(ItemRelationDto relationDto);
    Task<ItemRelationDto?> CreateItemRelation(ItemRelationDto relationDto);

    Task<ItemDto?> UpdateItem(ItemDto itemDto);
    Task<ItemDto?> CreateItem(ItemDto itemDto);
    Task<ItemRelationDto?> CreateRelation(int itemId, int? relatedItemId, int relationTypeId);


  }

  public class AppDataModuleService : IAppDataModuleService {    
    private readonly IServiceScopeFactory _scopeFactory;

    public AppDataModuleService(IServiceScopeFactory scopeFactory) { 
        _scopeFactory = scopeFactory;        
    }

    public async Task<List<ItemDto>> GetAllProjectItems() {
      var context = GetDbContext();

      var query = context.Items        
        .Where(i => i.ItemTypeId == (int)StItemType.Project && i.IsActive)
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
      await context.SaveChangesAsync();
      return relation.ToDto();
    }

    public async Task<ItemRelationDto?> CreateItemRelation(ItemRelationDto relationDto) {
      var context = GetDbContext();
      var relation = new ItemRelation {
        ItemId = relationDto.ItemId,
        RelatedItemId = relationDto.RelatedItemId,
        RelationTypeId = relationDto.RelationTypeId
      };
      context.ItemRelations.Add(relation);
      await context.SaveChangesAsync();
      return relation.ToDto();
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
      var relation = new ItemRelation {
        ItemId = itemId,
        RelatedItemId = relatedItemId,
        RelationTypeId = relationTypeId
      };
      context.ItemRelations.Add(relation);
      await context.SaveChangesAsync();
      return relation.ToDto();
    }

    private StorytimeDbContext GetDbContext() {
      var scope = _scopeFactory.CreateScope();
      return scope.ServiceProvider.GetRequiredService<StorytimeDbContext>();
    }
  }
}
