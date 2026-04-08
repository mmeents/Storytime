using KB.Core.Entities;
using KB.Core.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Storytime.Core.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;


namespace Storytime.Core.Handlers.Agents {

  // ─── Character Speak (MCP exposed) ───────────────────────────────────────────
  public record AddCharacterSpeakToPerformanceCommand(
    int PerformanceId,
    int CharacterId,
    string CharacterName,
    string Line
  ) : IRequest<ItemDto?>;

  public class AddCharacterSpeakToPerformanceCommandHandler(
    StorytimeDbContext context,
    ILogger<AddCharacterSpeakToPerformanceCommandHandler> logger
   ) : IRequestHandler<AddCharacterSpeakToPerformanceCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;
    private readonly ILogger<AddCharacterSpeakToPerformanceCommandHandler> _logger = logger;

    public async Task<ItemDto?> Handle(AddCharacterSpeakToPerformanceCommand request, CancellationToken cancellationToken) {
      if (request.PerformanceId <= 0) {
        _logger.LogError("Invalid PerformanceId: {PerformanceId}", request.PerformanceId);
        return null;
      }

      var performance = await _context.Items
          .FirstOrDefaultAsync(i => i.Id == request.PerformanceId && i.IsActive, cancellationToken);

      if (performance == null) {
        _logger.LogError("Performance with id {PerformanceId} not found", request.PerformanceId);
        throw new Exception($"Performance with id {request.PerformanceId} not found");
      }

      PerformanceScript script;
      try {
        script = string.IsNullOrWhiteSpace(performance.Data) || performance.Data == "{}"
           ? new PerformanceScript()
           : JsonSerializer.Deserialize<PerformanceScript>(performance.Data) ?? new PerformanceScript();
      } catch (JsonException ex) {
        _logger.LogError(ex, "Failed to deserialize PerformanceScript for PerformanceId {PerformanceId}: {Message}", request.PerformanceId, ex.Message);
        throw new Exception($"Failed to deserialize PerformanceScript for PerformanceId {request.PerformanceId}: {ex.Message}");
      }

      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {
        var nextRank = await _context.ItemRelations
            .Where(ir => ir.ItemId == request.PerformanceId)
            .CountAsync(cancellationToken) + 1;

        _context.ItemRelations.Add(new ItemRelation {
          ItemId = request.PerformanceId,
          RelationTypeId = (int)StRelationType.HasRole,
          RelatedItemId = request.CharacterId,
          Rank = nextRank
        });

        script.Entries.Add(new PerformanceEntry {
          Rank = nextRank,
          Type = "Speech",
          CharacterId = request.CharacterId,
          CharacterName = request.CharacterName,
          Text = request.Line
        });

        performance.Data = JsonSerializer.Serialize(script);
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
      } catch (Exception ex) {
        _logger.LogError(ex, "Failed to add character speak to performance with id {PerformanceId}: {Message}", request.PerformanceId, ex.Message);
        await transaction.RollbackAsync(cancellationToken);
        throw;
      }

      try {

        var result = await _context.Items
          .AsNoTracking()
          .Where(i => i.Id == request.PerformanceId && i.IsActive)
          .Include(i => i.ItemType)
          .FirstOrDefaultAsync(cancellationToken);

        return result?.ToDto(false);
      } catch (Exception ex) {
        _logger.LogError(ex, "Failed to retrieve updated performance with id {PerformanceId} after adding character speak: {Message}", request.PerformanceId, ex.Message);
        throw new Exception($"Failed to retrieve updated performance with id {request.PerformanceId} after adding character speak: {ex.Message}");
      }
    }
  }

}
