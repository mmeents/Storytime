using KB.Core.Entities;
using KB.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Storytime.Core.Handlers.Agents {
  public record AddSceneToStoryCommand(
    int StoryId,
    string Name,
    string Description
  ) : IRequest<ItemDto?>;

  public class AddSceneToStoryCommandHandler(
    StorytimeDbContext context, 
    ILogger<AddSceneToStoryCommandHandler> logger
  ) : IRequestHandler<AddSceneToStoryCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;
    private readonly ILogger<AddSceneToStoryCommandHandler> _logger = logger;

    public async Task<ItemDto?> Handle(AddSceneToStoryCommand request, CancellationToken cancellationToken) {

      if (request.StoryId <= 0){ 
        _logger.LogError("Invalid StoryId: {StoryId}", request.StoryId);
        return null; 
      }
      var parentExists = await _context.Items.AnyAsync(i => i.Id == request.StoryId && i.IsActive, cancellationToken);
      if (!parentExists) {
        _logger.LogError("Parent item with id {StoryId} not found", request.StoryId);
        throw new Exception($"Parent item with id {request.StoryId} not found");
      }

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
        _logger.LogError("Error occurred while adding scene to story with id {StoryId}", request.StoryId);
        await transaction.RollbackAsync(cancellationToken);
        throw;
      }
      try { 

        return await _context.Items
          .AsNoTracking()
          .Where(i => i.Id == newRelatedItem.Id && i.IsActive)  
          .Include(i => i.ItemType)
          .Include(i => i.Relations).ThenInclude(r => r.RelatedItem)
          .Include(i => i.Relations).ThenInclude(r => r.RelationType)
          .Include(i => i.IncomingRelations).ThenInclude(r => r.Item)
          .Include(i => i.IncomingRelations).ThenInclude(r => r.RelationType)
          .FirstOrDefaultAsync(cancellationToken)
          .ContinueWith(t => t.Result?.ToDto(true), cancellationToken);

      } catch (Exception ex) {
        _logger.LogError(ex, "Error occurred while retrieving new scene with id {SceneId}", newRelatedItem.Id);
        throw;
      }

    }
  }
}
