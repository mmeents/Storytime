using KB.Core.Entities;
using KB.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Storytime.Core.Handlers.Agents {

  public record AddDeliverableForPerformanceCommand(
  int PerformanceId,
  string Name,
  string Content
) : IRequest<ItemDto?>;

  public class AddDeliverableForPerformanceCommandHandler(
    StorytimeDbContext context,
    ILogger<AddDeliverableForPerformanceCommandHandler> logger
  ) : IRequestHandler<AddDeliverableForPerformanceCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;
    private readonly ILogger<AddDeliverableForPerformanceCommandHandler> _logger = logger;

    public async Task<ItemDto?> Handle(AddDeliverableForPerformanceCommand request, CancellationToken cancellationToken) {
      if (request.PerformanceId <= 0) {
        _logger.LogError("Invalid PerformanceId: {PerformanceId}", request.PerformanceId);
        return null;
      }

      var performanceExists = await _context.Items
          .AnyAsync(i => i.Id == request.PerformanceId && i.IsActive, cancellationToken);

      if (!performanceExists) {
        _logger.LogError("Performance with id {PerformanceId} not found", request.PerformanceId);
        throw new Exception($"Performance with id {request.PerformanceId} not found");
      }

      var deliverable = new Item {
        Name = request.Name,
        ItemTypeId = (int)StItemType.Deliverable,
        Description = request.Content,   // prose lives in Description — human readable, no JSON
        Data = "{}",
        IsActive = true
      };

      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {
        _context.Items.Add(deliverable);
        await _context.SaveChangesAsync(cancellationToken);

        _context.ItemRelations.Add(new ItemRelation {
          ItemId = request.PerformanceId,
          RelationTypeId = (int)StRelationType.Produces,
          RelatedItemId = deliverable.Id
        });

        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
      } catch (Exception ex) {
        await transaction.RollbackAsync(cancellationToken);
        _logger.LogError(ex, "Error occurred while adding deliverable for performance with id {PerformanceId}", request.PerformanceId);
        throw;
      }

      try { 

        var result = await _context.Items
            .AsNoTracking()
            .Where(i => i.Id == deliverable.Id && i.IsActive)
            .Include(i => i.ItemType)
            .FirstOrDefaultAsync(cancellationToken);

        return result?.ToDto(false);

      } catch (Exception ex) {
        _logger.LogError(ex, "Failed to retrieve new deliverable with id {DeliverableId} after adding to performance with id {PerformanceId}: {Message}", request.PerformanceId, request.PerformanceId, ex.Message);
        throw new Exception($"Failed to retrieve new deliverable with id {request.PerformanceId} after adding to performance with id {request.PerformanceId}: {ex.Message}");
      }
    }

  }
}
