using KB.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Storytime.Core.Models;
using System.Text.Json;
using Storytime.Core.Constants;


namespace Storytime.Core.Handlers.Agents {

  public record AddNarrationToPerformanceCommand(
    int PerformanceId,
    string Section,
    string Text
  ) : IRequest<ItemDto?>;

  public class AddNarrationToPerformanceCommandHandler(
    StorytimeDbContext context,
    ILogger<AddNarrationToPerformanceCommandHandler> logger)
      : IRequestHandler<AddNarrationToPerformanceCommand, ItemDto?> {
    private readonly StorytimeDbContext _context = context;
    private readonly ILogger<AddNarrationToPerformanceCommandHandler> _logger = logger;

    public async Task<ItemDto?> Handle(AddNarrationToPerformanceCommand request, CancellationToken cancellationToken) {
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

      try {

        var script = string.IsNullOrWhiteSpace(performance.Data) || performance.Data == "{}"
            ? new PerformanceScript()
            : JsonSerializer.Deserialize<PerformanceScript>(performance.Data) ?? new PerformanceScript();

        var nextRank = script.Entries.Count + 1;

        script.Entries.Add(new PerformanceEntry {
          Rank = nextRank,
          Type = "Narration",
          CharacterId = null,
          CharacterName = "Narrator",
          Text = request.Text
        });

        performance.Data = JsonSerializer.Serialize(script);
        _context.Items.Update(performance);
        await _context.SaveChangesAsync(cancellationToken);

        var result = await _context.GetMinimalItemDtoById(request.PerformanceId, cancellationToken);

        return result;

      } catch (Exception ex) {
        _logger.LogError(ex, "Failed to add narration to performance with id {PerformanceId}: {Message}", request.PerformanceId, ex.Message);
        throw new Exception($"Failed to add narration to performance with id {request.PerformanceId}: {ex.Message}");
      }


    }
  }
}
