using KB.Core.Entities;
using KB.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Storytime.Core.Models;
using System.Text.Json;
using Storytime.Core.Constants;


namespace Storytime.Core.Handlers.Agents {

  public record AddPerformanceForCallSheetCommand(
    int CallSheetId,
    string Name,
    string Description
  ) : IRequest<ItemDto?>;


  public class AddPerformanceForCallSheetCommandHandler(
    StorytimeDbContext context,
    ILogger<AddPerformanceForCallSheetCommandHandler> logger
  ) : IRequestHandler<AddPerformanceForCallSheetCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;
    private readonly ILogger<AddPerformanceForCallSheetCommandHandler> _logger = logger;

    public async Task<ItemDto?> Handle(AddPerformanceForCallSheetCommand request, CancellationToken cancellationToken) {

      if (request.CallSheetId <= 0) {
        _logger.LogError("Invalid CallSheetId: {CallSheetId}", request.CallSheetId);
        return null;
      }

      var callSheetExists = await _context.Items
        .AnyAsync(i => i.Id == request.CallSheetId && i.IsActive, cancellationToken);

      if (!callSheetExists) {
        _logger.LogError("CallSheet with id {CallSheetId} not found", request.CallSheetId);
        throw new Exception($"CallSheet with id {request.CallSheetId} not found");
      }

      var performance = new Item {
        Name = request.Name,
        ItemTypeId = (int)StItemType.Performance,
        Description = request.Description,
        Data = JsonSerializer.Serialize(new PerformanceScript()),
        IsActive = true
      };

      

      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {
        var nextRank = await _context.GetItemsNextRankId(request.CallSheetId, cancellationToken);

        _context.Items.Add(performance);
        await _context.SaveChangesAsync(cancellationToken);

        _context.ItemRelations.Add(new ItemRelation {
          ItemId = request.CallSheetId,
          Rank = nextRank,
          RelationTypeId = (int)StRelationType.Produces,
          RelatedItemId = performance.Id
        });

        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

      } catch (Exception ex) {
        _logger.LogError(ex, "Error occurred while adding performance for CallSheetId: {CallSheetId}", request.CallSheetId);
        await transaction.RollbackAsync(cancellationToken);
        throw;
      }

      try {
        // Return the new Performance DTO directly — caller gets .Id immediately
        return await _context.GetMinimalItemDtoById(request.CallSheetId, cancellationToken);

      } catch (Exception ex) {
        _logger.LogError(ex, "Failed to retrieve new performance with id {PerformanceId} after adding for CallSheetId {CallSheetId}: {Message}", performance.Id, request.CallSheetId, ex.Message);
        throw new Exception($"Failed to retrieve new performance with id {performance.Id} after adding for CallSheetId {request.CallSheetId}: {ex.Message}");
      }
    }
  }


}
