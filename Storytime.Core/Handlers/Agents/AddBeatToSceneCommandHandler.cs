using KB.Core.Entities;
using KB.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Storytime.Core.Handlers.Agents {

  public record AddBeatToSceneCommand(
      int SceneId,
      string Name,
      string Description
  ) : IRequest<ItemDto?>;

  public class AddBeatToSceneCommandHandler(
    StorytimeDbContext context,
    ILogger<AddBeatToSceneCommandHandler> logger
  ) : IRequestHandler<AddBeatToSceneCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;
    private readonly ILogger<AddBeatToSceneCommandHandler> _logger = logger;

    public async Task<ItemDto?> Handle(AddBeatToSceneCommand request, CancellationToken cancellationToken) {
      if (request.SceneId <= 0){
        _logger.LogError("Invalid SceneId: {SceneId}", request.SceneId);
        return null;
      }
      var parentExists = await _context.Items.AnyAsync(i => i.Id == request.SceneId && i.IsActive, cancellationToken);
      if (!parentExists) {
        _logger.LogError("Parent item with id {SceneId} not found", request.SceneId);
        throw new Exception($"Parent item with id {request.SceneId} not found");
      }

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

      } catch (Exception ex) {
        _logger.LogError("Error adding beat to scene {SceneId}: {ErrorMessage}", request.SceneId, ex.Message);
        await transaction.RollbackAsync(cancellationToken);
        throw;
      }
      try { 

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

      } catch (Exception ex) {
        _logger.LogError("Error retrieving new beat with id {BeatId}: {ErrorMessage}", newRelatedItem.Id, ex.Message);
        throw;
      }
    }
  }

}
