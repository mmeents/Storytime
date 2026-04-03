using KB.Core.Models;
using MediatR;
using Storytime.Core.Handlers.Agents;
using Storytime.Core.Models;
using Storytime.Core.Handlers.Items;
using System.Text.Json;
using Storytime.Core.Constants;

namespace Storytime.Core.Agents {
  public interface IObserverAgent {
    Task<ItemDto?> ObservePerformance(int performanceId, CancellationToken ct = default);
  }

  public class ObserverAgent : IObserverAgent {
    private readonly ILocalBaseAgent _baseAgent;
    private readonly IMediator _mediator;

    public ObserverAgent(ILocalBaseAgentFactory baseAgentFactory, IMediator mediator) {
      // No MCP tools — Observer just reads and writes prose
      _baseAgent = baseAgentFactory.Create();
      _baseAgent.Name = $"Olivia the Observer at {Cx.AppName}";
      _mediator = mediator;
    }

    public async Task<ItemDto?> ObservePerformance(int performanceId, CancellationToken ct = default) {

      var performance = await _mediator.Send(new GetItemByIdQuery(performanceId, false), ct);
      if (performance == null)
        throw new Exception($"Performance with id {performanceId} not found.");

      if (string.IsNullOrWhiteSpace(performance.Data) || performance.Data == "{}")
        throw new Exception($"Performance {performanceId} has no entries. Run the Set first.");

      var script = JsonSerializer.Deserialize<PerformanceScript>(performance.Data)
          ?? throw new Exception($"Failed to deserialize Performance {performanceId} entries.");

      if (!script.Entries.Any())
        throw new Exception($"Performance {performanceId} has no entries.");

      // Format the performance entries into a readable transcript for the LM
      var transcript = string.Join("\n\n", script.Entries
          .OrderBy(e => e.Rank)
          .Select(e => e.Type == "Narration"
              ? $"[Scene] {e.Text}"
              : $"[{e.CharacterName} — {e.Type}] {e.Text}"));

      _baseAgent.SystemPrompt =
          $"You are Olivia the Observer at {Cx.AppName}." + Environment.NewLine +
          "You have watched a scene perform and your job is to translate it into flowing prose." + Environment.NewLine +
          "You will be given a transcript of what happened — narration, speech, and actions." + Environment.NewLine +
          "Weave it into a single cohesive piece of writing. " +
          "Preserve the voices, the atmosphere, and the dramatic arc." + Environment.NewLine +
          "Write only the prose — no headings, no transcript labels, no commentary. " +
          "Just the story as it happened, beautifully told.";

      _baseAgent.UserPrompt =
          $"Performance: {performance.Name}\n\n" +
          $"Transcript:\n{transcript}\n\n" +
          "Translate this into flowing prose. You got this, thanks.";

      var response = await _baseAgent.InvokeAgentAsync(ct);

      var prose = response?.GetText()?.Trim();
      if (string.IsNullOrWhiteSpace(prose))
        throw new Exception("Observer returned empty response.");

      // Prose lives in Description — directly readable in the tree, no JSON needed
      var deliverable = await _mediator.Send(
          new AddDeliverableForPerformanceCommand(
              performanceId,
              $"{performance.Name.Replace(" - Performance", "")}",
              prose
          ), ct);

      return deliverable;
    }
  }
}
