using KB.Core.Entities;
using KB.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Storytime.Core.Constants;
using Storytime.Core.Models;
using System.Text.Json;


namespace Storytime.Core.Handlers.Agents {



  public record AddNarrationToCallSheetCommand(
    int CallSheetId,
    string Section,
    string Narration
  ) : IRequest<ItemDto?>;

  public class AddNarrationToCallSheetCommandHandler(
    StorytimeDbContext context,
    ILogger<AddNarrationToCallSheetCommandHandler> logger
  ) : IRequestHandler<AddNarrationToCallSheetCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;
    private readonly ILogger<AddNarrationToCallSheetCommandHandler> _logger = logger;
    public async Task<ItemDto?> Handle(AddNarrationToCallSheetCommand request, CancellationToken cancellationToken) {
    
      if (request.CallSheetId <= 0){ 
        _logger.LogError("Invalid CallSheetId: {CallSheetId}", request.CallSheetId);
        return null; }
    
      var callSheet = await _context.Items
          .FirstOrDefaultAsync(i => i.Id == request.CallSheetId && i.IsActive, cancellationToken);
      if (callSheet == null) {
        _logger.LogError("Parent item with id {CallSheetId} not found", request.CallSheetId);
        throw new Exception($"Parent item with id {request.CallSheetId} not found");
      }
    
      var script = string.IsNullOrWhiteSpace(callSheet.Data) || callSheet.Data == "{}"
          ? new CallSheetScript()
          : JsonSerializer.Deserialize<CallSheetScript>(callSheet.Data) ?? new CallSheetScript();
      Item newRelatedItem;
      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {

        var nextRank = await _context.GetItemsNextRankId(request.CallSheetId, cancellationToken);

        newRelatedItem = new Item {
          Name = request.Section,
          ItemTypeId = (int)StItemType.Narration,
          Description = request.Narration,
          Data = "{}",
          IsActive = true
        };
        _context.Items.Add(newRelatedItem);
        await _context.SaveChangesAsync(cancellationToken);

        _context.ItemRelations.Add(new ItemRelation {
          ItemId = request.CallSheetId,
          RelationTypeId = (int)StRelationType.Narrates,
          RelatedItemId = newRelatedItem.Id,
          Rank = nextRank
        });

        script.Script.Add(new CharacterPrompt {
          Rank = nextRank,    
          Type = "Narration",
          Name = request.Section,
          Instruction = request.Narration
        });

        callSheet.Data = JsonSerializer.Serialize(script);
        await _context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

      } catch {
        _logger.LogError("Error adding narration to call sheet with id {CallSheetId}", request.CallSheetId);
        await transaction.RollbackAsync(cancellationToken);
        throw;
      }

      return await _context.GetMinimalItemDtoById(newRelatedItem.Id, cancellationToken);

    }
  }




}
