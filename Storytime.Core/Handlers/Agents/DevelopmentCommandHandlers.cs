using KB.Core.Entities;
using KB.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Storytime.Core.Models;

namespace Storytime.Core.Handlers.Agents {
  public record AddStoryToProjectCommand(
    int ProjectId,
    string Name,
    string Description
  ) : IRequest<ItemDto?>;

  public class DevelopmentCommandHandlers(StorytimeDbContext context): IRequestHandler<AddStoryToProjectCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;
    public async Task<ItemDto?> Handle(AddStoryToProjectCommand request, CancellationToken cancellationToken) {
      if (request.ProjectId <= 0) return null;
      var parentExists = await _context.Items.AnyAsync(i => i.Id == request.ProjectId && i.IsActive, cancellationToken);
      if (!parentExists) throw new Exception($"Parent item with id {request.ProjectId} not found");
      
      var newRelatedItem = new Item {
        Name = request.Name,
        ItemTypeId = (int)StItemType.Story,
        Description = request.Description,
        Data = "{}",
        IsActive = true
      };

      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {
        _context.Items.Add(newRelatedItem);
        await _context.SaveChangesAsync(cancellationToken);

        var nextRank = await _context.ItemRelations
          .Where(ir => ir.ItemId == request.ProjectId)
          .CountAsync(cancellationToken) + 1;

        _context.ItemRelations.Add(new ItemRelation {
          ItemId = request.ProjectId,
          RelationTypeId = (int)StRelationType.Contains,
          RelatedItemId = newRelatedItem.Id,
          Rank = nextRank
        });
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
      } catch {
        await transaction.RollbackAsync(cancellationToken);
        throw;
      }

      return await _context.Items
        .AsNoTracking()
        .Where(i => i.Id == newRelatedItem.Id && i.IsActive)  // ← parent, as you had it
        .Include(i => i.ItemType)
        .Include(i => i.Relations)
            .ThenInclude(r => r.RelatedItem)
        .Include(i => i.Relations)
            .ThenInclude(r => r.RelationType)
        .Include(i => i.IncomingRelations)
            .ThenInclude(r => r.Item)
        .Include(i => i.IncomingRelations)
            .ThenInclude(r => r.RelationType)
        .FirstOrDefaultAsync(cancellationToken)
        .ContinueWith(t => t.Result?.ToDto(true), cancellationToken);
    }
  }

  public record AddSceneToStoryCommand(
    int StoryId,
    string Name,
    string Description
  ) : IRequest<ItemDto?>;

  public class AddSceneToStoryCommandHandler(StorytimeDbContext context) : IRequestHandler<AddSceneToStoryCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;
    public async Task<ItemDto?> Handle(AddSceneToStoryCommand request, CancellationToken cancellationToken) {
      if (request.StoryId <= 0) return null;
      var parentExists = await _context.Items.AnyAsync(i => i.Id == request.StoryId && i.IsActive, cancellationToken);
      if (!parentExists) throw new Exception($"Parent item with id {request.StoryId} not found");
      
      var newRelatedItem = new Item {
        Name = request.Name,
        ItemTypeId = (int)StItemType.Scene,
        Description = request.Description,
        Data = "{}",
        IsActive = true
      };
      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {
        _context.Items.Add(newRelatedItem);
        await _context.SaveChangesAsync(cancellationToken);

        var nextRank = await _context.ItemRelations
          .Where(ir => ir.ItemId == request.StoryId)
          .CountAsync(cancellationToken) + 1;

        _context.ItemRelations.Add(new ItemRelation {
          ItemId = request.StoryId,
          RelationTypeId = (int)StRelationType.Contains,
          RelatedItemId = newRelatedItem.Id,
          Rank = nextRank
        });
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
      } catch {
        await transaction.RollbackAsync(cancellationToken);
        throw;
      }
      return await _context.Items
        .AsNoTracking()
        .Where(i => i.Id == newRelatedItem.Id && i.IsActive)  // ← parent, as you had it
        .Include(i => i.ItemType)
        .Include(i => i.Relations)
            .ThenInclude(r => r.RelatedItem)
        .Include(i => i.Relations)
            .ThenInclude(r => r.RelationType)
        .Include(i => i.IncomingRelations)
            .ThenInclude(r => r.Item)
        .Include(i => i.IncomingRelations)
            .ThenInclude(r => r.RelationType)
        .FirstOrDefaultAsync(cancellationToken)
        .ContinueWith(t => t.Result?.ToDto(true), cancellationToken);
    }
  }

  public record AddBeatToSceneCommand(
      int SceneId,
      string Name,
      string Description
  ) : IRequest<ItemDto?>;

  public class AddBeatToSceneCommandHandler(StorytimeDbContext context) : IRequestHandler<AddBeatToSceneCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;
    public async Task<ItemDto?> Handle(AddBeatToSceneCommand request, CancellationToken cancellationToken) {
      if (request.SceneId <= 0) return null;
      var parentExists = await _context.Items.AnyAsync(i => i.Id == request.SceneId && i.IsActive, cancellationToken);
      if (!parentExists) throw new Exception($"Parent item with id {request.SceneId} not found");
      var newRelatedItem = new Item {
        Name = request.Name,
        ItemTypeId = (int)StItemType.Beat,
        Description = request.Description,
        Data = "{}",
        IsActive = true
      };

      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {
        _context.Items.Add(newRelatedItem);
        await _context.SaveChangesAsync(cancellationToken);
        var nextRank = await _context.ItemRelations
          .Where(ir => ir.ItemId == request.SceneId)
          .CountAsync(cancellationToken) + 1;
        _context.ItemRelations.Add(new ItemRelation {
          ItemId = request.SceneId,
          RelationTypeId = (int)StRelationType.Contains,
          RelatedItemId = newRelatedItem.Id,
          Rank = nextRank
        });
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
      } catch {
        await transaction.RollbackAsync(cancellationToken);
        throw;
      }

      return await _context.Items
        .AsNoTracking()
        .Where(i => i.Id == newRelatedItem.Id && i.IsActive) 
        .Include(i => i.ItemType)
        .Include(i => i.Relations)
            .ThenInclude(r => r.RelatedItem)
        .Include(i => i.Relations)
            .ThenInclude(r => r.RelationType)
        .Include(i => i.IncomingRelations)
            .ThenInclude(r => r.Item)
        .Include(i => i.IncomingRelations)
            .ThenInclude(r => r.RelationType)
        .FirstOrDefaultAsync(cancellationToken)
        .ContinueWith(t => t.Result?.ToDto(true), cancellationToken);
    }
  }

  public record AddCharacterToStoryCommand(
      int StoryId,
      string Name,
      string Description
  ) : IRequest<ItemDto?>;

  public class AddCharacterToStoryCommandHandler(StorytimeDbContext context) : IRequestHandler<AddCharacterToStoryCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;
    public async Task<ItemDto?> Handle(AddCharacterToStoryCommand request, CancellationToken cancellationToken) {
      if (request.StoryId <= 0) return null;
      var parentExists = await _context.Items.AnyAsync(i => i.Id == request.StoryId && i.IsActive, cancellationToken);
      if (!parentExists) throw new Exception($"Parent item with id {request.StoryId} not found");
      var newRelatedItem = new Item {
        Name = request.Name,
        ItemTypeId = (int)StItemType.Character,
        Description = request.Description,
        Data = "{}",
        IsActive = true
      };
      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {
          _context.Items.Add(newRelatedItem);
          await _context.SaveChangesAsync(cancellationToken);
          var nextRank = await _context.ItemRelations
            .Where(ir => ir.ItemId == request.StoryId)
            .CountAsync(cancellationToken) + 1;
          _context.ItemRelations.Add(new ItemRelation {
              ItemId = request.StoryId,
              RelationTypeId = (int)StRelationType.FeaturesCharacter,
              RelatedItemId = newRelatedItem.Id
          });
          await _context.SaveChangesAsync(cancellationToken);
          await transaction.CommitAsync(cancellationToken);
      } catch {
          await transaction.RollbackAsync(cancellationToken);
          throw;
      }
      return await _context.Items
        .AsNoTracking()
        .Where(i => i.Id == newRelatedItem.Id && i.IsActive)  // ← parent, as you had it
        .Include(i => i.ItemType)
        .Include(i => i.Relations)
            .ThenInclude(r => r.RelatedItem)
        .Include(i => i.Relations)
            .ThenInclude(r => r.RelationType)
        .Include(i => i.IncomingRelations)
            .ThenInclude(r => r.Item)
        .Include(i => i.IncomingRelations)
            .ThenInclude(r => r.RelationType)
        .FirstOrDefaultAsync(cancellationToken)
        .ContinueWith(t => t.Result?.ToDto(true), cancellationToken);
    }

  }


}   
