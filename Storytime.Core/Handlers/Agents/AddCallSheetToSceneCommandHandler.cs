using KB.Core.Entities;
using KB.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Storytime.Core.Constants;


namespace Storytime.Core.Handlers.Agents {
  public record AddCallSheetToSceneCommand(
      int SceneId,
      string Name,
      string Description
    ) : IRequest<ItemDto?>;

  // This returns the CallSheet since it's only being used internally and not by a mcp server.
  public class AddCallSheetToSceneCommandHandler(
    StorytimeDbContext context, 
    ILogger<AddCallSheetToSceneCommandHandler> logger
  ) : IRequestHandler<AddCallSheetToSceneCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;
    private readonly ILogger<AddCallSheetToSceneCommandHandler> _logger = logger;
    public async Task<ItemDto?> Handle(AddCallSheetToSceneCommand request, CancellationToken cancellationToken) {
      if (request.SceneId <= 0) {
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
        ItemTypeId = (int)StItemType.CallSheet,
        Description = request.Description,
        Data = "{}",
        IsActive = true
      };

      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {
        _context.Items.Add(newRelatedItem);
        await _context.SaveChangesAsync(cancellationToken);

        var nextRank = await _context.GetItemsNextRankId(request.SceneId, cancellationToken);

        _context.ItemRelations.Add(new ItemRelation {
          ItemId = request.SceneId,
          RelationTypeId = (int)StRelationType.DirectedAs,
          RelatedItemId = newRelatedItem.Id,
          Rank = nextRank
        });
        await _context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
      } catch {
        await transaction.RollbackAsync(cancellationToken);
        _logger.LogError("Error occurred while adding CallSheet to SceneId: {SceneId}", request.SceneId);
        throw;
      }

      try {

        var result = await _context.GetItemDtoById(newRelatedItem.Id, cancellationToken);
        return result;

      } catch (Exception ex) {
        _logger.LogError(ex, "Error occurred while retrieving CallSheet with id: {CallSheetId}", newRelatedItem.Id);
        throw;
      }
    }
  }

}
