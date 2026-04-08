using KB.Core.Entities;
using KB.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Storytime.Core.Models;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Storytime.Core.Constants;

namespace Storytime.Core.Handlers.Agents {

  public record AddRoleToCallSheetCommand(
    int CallSheetId,
    int CharacterId,
    string Name,
    string Instruction
  ) : IRequest<ItemDto?>;

  public class AddRoleToCallSheetCommandHandler(
     StorytimeDbContext context, 
     ILogger<AddRoleToCallSheetCommandHandler> logger
  ) : IRequestHandler<AddRoleToCallSheetCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;
    private readonly ILogger<AddRoleToCallSheetCommandHandler> _logger = logger;

    public async Task<ItemDto?> Handle(AddRoleToCallSheetCommand request, CancellationToken cancellationToken) {

      if (request.CallSheetId <= 0) {
        _logger.LogError("Invalid CallSheetId: {CallSheetId}", request.CallSheetId);
        return null;
      }

      var callSheet = await _context.Items
        .FirstOrDefaultAsync(i => i.Id == request.CallSheetId && i.IsActive, cancellationToken);

      if (callSheet == null) {
        _logger.LogError("Parent item with id {CallSheetId} not found", request.CallSheetId);
        throw new Exception($"Parent item with id {request.CallSheetId} not found");
      }


      var script = string.IsNullOrWhiteSpace(callSheet.Data) || callSheet.Data == "{}"
        ? new CallSheetScript()
        : JsonSerializer.Deserialize<CallSheetScript>(callSheet.Data) ?? new CallSheetScript();

      var characterExists = await _context.Items.AnyAsync(i => i.Id == request.CharacterId && i.IsActive, cancellationToken);
      if (!characterExists) {
        _logger.LogError("Character item with id {CharacterId} not found", request.CharacterId);
        throw new Exception($"Character item with id {request.CharacterId} not found");
      }
            
      try {
        var nextRank = await _context.GetItemsNextRankId(request.CallSheetId, cancellationToken);

        var existingRelation = await _context.ItemRelations
          .AnyAsync(ir => ir.ItemId == request.CallSheetId
            && ir.RelationTypeId == (int)StRelationType.HasRole
            && ir.RelatedItemId == request.CharacterId, cancellationToken);

        if (!existingRelation) {
          var newRelation = new ItemRelation {
            ItemId = request.CallSheetId,
            RelationTypeId = (int)StRelationType.HasRole,
            RelatedItemId = request.CharacterId,
            Rank = nextRank
          };
          _context.ItemRelations.Add(newRelation);
        }       

        script.Script.Add(new CharacterPrompt {
          Rank = nextRank,
          CharacterId = request.CharacterId,
          Name = request.Name,
          Instruction = request.Instruction
        });

        callSheet.Data = JsonSerializer.Serialize(script);

        await _context.SaveChangesAsync(cancellationToken);
        

      } catch {
        _logger.LogError("Error adding role to call sheet {CallSheetId} for character {CharacterId}", request.CallSheetId, request.CharacterId);        
        throw;
      }

      try { 

        return await _context.GetMinimalItemDtoById(request.CallSheetId, cancellationToken);          

      } catch (Exception ex) {
        _logger.LogError(ex, "Error retrieving updated call sheet with id {CallSheetId}", request.CallSheetId);
        throw new Exception($"Error retrieving updated call sheet with id {request.CallSheetId}", ex);
      }
    }



  }
}
