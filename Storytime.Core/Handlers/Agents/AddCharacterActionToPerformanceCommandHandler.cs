using KB.Core.Entities;
using KB.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Storytime.Core.Models;
using System.Text.Json;


namespace Storytime.Core.Handlers.Agents {

  // ─── Character Action (MCP exposed) ──────────────────────────────────────────

  public record AddCharacterActionToPerformanceCommand(
    int PerformanceId,
    int CharacterId,
    string CharacterName,
    string Action
  ) : IRequest<ItemDto?>;

  public class AddCharacterActionToPerformanceCommandHandler(StorytimeDbContext context)
      : IRequestHandler<AddCharacterActionToPerformanceCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;

    public async Task<ItemDto?> Handle(AddCharacterActionToPerformanceCommand request, CancellationToken cancellationToken) {
      if (request.PerformanceId <= 0) return null;

      var performance = await _context.Items
          .FirstOrDefaultAsync(i => i.Id == request.PerformanceId && i.IsActive, cancellationToken);
      if (performance == null)
        throw new Exception($"Performance with id {request.PerformanceId} not found");

      var script = string.IsNullOrWhiteSpace(performance.Data) || performance.Data == "{}"
          ? new PerformanceScript()
          : JsonSerializer.Deserialize<PerformanceScript>(performance.Data) ?? new PerformanceScript();

      using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
      try {
        var nextRank = await _context.ItemRelations
            .Where(ir => ir.ItemId == request.PerformanceId)
            .CountAsync(cancellationToken) + 1;

        var existingRelation = await _context.ItemRelations
          .AnyAsync(ir => ir.ItemId == request.PerformanceId
            && ir.RelationTypeId == (int)StRelationType.HasRole
            && ir.RelatedItemId == request.CharacterId, cancellationToken);

        if (!existingRelation) {
          _context.ItemRelations.Add(new ItemRelation {
            ItemId = request.PerformanceId,
            RelationTypeId = (int)StRelationType.HasRole,
            RelatedItemId = request.CharacterId,
            Rank = nextRank
          });
        }

        script.Entries.Add(new PerformanceEntry {
          Rank = nextRank,
          Type = "Action",
          CharacterId = request.CharacterId,
          CharacterName = request.CharacterName,
          Text = request.Action
        });

        performance.Data = JsonSerializer.Serialize(script);
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
      } catch {
        await transaction.RollbackAsync(cancellationToken);
        throw;
      }

      var result = await _context.Items
          .AsNoTracking()
          .Where(i => i.Id == request.PerformanceId && i.IsActive)
          .Include(i => i.ItemType)
          .FirstOrDefaultAsync(cancellationToken);

      return result?.ToDto(false);
    }
  }

}
