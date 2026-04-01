using MediatR;
using Storytime.Core.Handlers.Agents;
using Storytime.Core.Handlers.AsGraph;
using Storytime.Core.Handlers.Items;
using Storytime.Core.Models;
using Storytime.Core.Constants;
using System.Text.Json;

namespace Storytime.Core.Agents {
  public interface ISetAgent {
    Task PerformScene(int callSheetId, int storyId, CancellationToken ct = default);
  }

  public class SetAgent : ISetAgent {
    private readonly ILocalBaseAgent _baseAgent;
    private readonly IMediator _mediator;

    public SetAgent(ILocalBaseAgentFactory baseAgentFactory, IMediator mediator) {
      _baseAgent = baseAgentFactory.Create();
      _baseAgent.ToolsToUse.Add(Cx.LMStudioStorytimeMcpToolName);
      _baseAgent.Name = $"The Set at {Cx.AppName}";
      _mediator = mediator;
    }

    public async Task PerformScene(int callSheetId, int storyId, CancellationToken ct = default) {

      // Pre-load CallSheet — we need the Script JSON and scene name
      var callSheet = await _mediator.Send(new GetSubgraphQuery(callSheetId, 1), ct);
      if (callSheet.Root == null)
        throw new Exception($"CallSheet with id {callSheetId} not found.");

      if (string.IsNullOrWhiteSpace(callSheet.Root.Data) || callSheet.Root.Data == "{}")
        throw new Exception($"CallSheet {callSheetId} has no Script. Run the Director first.");

      var script = JsonSerializer.Deserialize<CallSheetScript>(callSheet.Root.Data)
          ?? throw new Exception($"Failed to deserialize CallSheet {callSheetId} Script.");

      if (!script.Script.Any())
        throw new Exception($"CallSheet {callSheetId} Script is empty.");

      // Pre-load story characters — keyed by Id for fast lookup during the loop
      var story = await _mediator.Send(new GetSubgraphQuery(storyId, 1), ct);
      if (story.Root == null)
        throw new Exception($"Story with id {storyId} not found.");

      var characterMap = story.Nodes
          .Where(n => n.Item.ItemTypeId == (int)StItemType.Character)
          .ToDictionary(n => n.Item.Id, n => n.Item);

      // C# creates the Performance — LM never needs to know this Id directly
      var performanceDto = await _mediator.Send(
          new AddPerformanceForCallSheetCommand(
              callSheetId,
              $"{callSheet.Root.Name.Replace(" - Call Sheet", "")} - Performance",
              $"Live performance generated from call sheet: {callSheet.Root.Name}"
          ), ct);

      if (performanceDto == null)
        throw new Exception($"Failed to create Performance for CallSheet {callSheetId}.");

      var performanceId = performanceDto.Id;

      // ── Main loop — DB is the shared state ──────────────────────────────────
      foreach (var entry in script.Script.OrderBy(e => e.Rank)) {

        // ── Narration — Director's words pass straight through ───────────────
        if (entry.Type == "Narration") {
          await _mediator.Send(new AddNarrationToPerformanceCommand(
              performanceId,
              entry.Name,
              entry.Instruction
          ), ct);
          continue;
        }

        // ── Role — spin up a character LM invocation ─────────────────────────
        if (entry.CharacterId == null) continue;


        // Reload Performance.Data fresh from DB — captures all prior MCP writes
        var freshPerformance = await _mediator.Send( new GetItemByIdQuery(performanceId), ct);
        var performanceSoFar = "";
        if (freshPerformance != null && 
          !string.IsNullOrWhiteSpace(freshPerformance.Data) && freshPerformance.Data != "{}") 
        {
          var performanceScript = JsonSerializer.Deserialize<PerformanceScript>(freshPerformance.Data);
          if (performanceScript?.Entries.Any() == true) {
            var lines = performanceScript.Entries.OrderBy(e => e.Rank)
              .Select(e => e.Type == "Narration" ? $"[Scene] {e.Text}" : $"[{e.CharacterName}] {e.Text}");
            performanceSoFar = string.Join("\n", lines);
          }
        }

        
        characterMap.TryGetValue(entry.CharacterId.Value, out var character);
        var characterDescription = character?.Description ?? "A character in the scene.";

        var contextBlock = string.IsNullOrWhiteSpace(performanceSoFar)
            ? "You are opening the scene."
            : $"Scene so far:\n{performanceSoFar}";

        _baseAgent.SystemPrompt =
            $"You are {entry.Name}, a character in {Cx.AppName}." + Environment.NewLine +
            $"Who you are: {characterDescription}" + Environment.NewLine +
            $"Director's instruction for this moment: {entry.Instruction}" + Environment.NewLine +
            "Express yourself using the tools available:" + Environment.NewLine +
            $"  {Cx.CmdAddCharacterSpeak} — say something out loud." + Environment.NewLine +
            $"  {Cx.CmdAddCharacterAction} — do something, react, perform." + Environment.NewLine +
            "Make your tool calls first. Then after your calls, respond in character — " +
            "a brief inner thought, reaction, or closing beat. Stay in character. " +
            "No JSON or curly brackets in tool parameters. Thanks.";

        _baseAgent.UserPrompt =
            $"{contextBlock}\n\n" +
            $"Performance Id: {performanceId}\n" +
            $"Your Character Id: {entry.CharacterId}\n" +
            $"Your Name: {entry.Name}\n\n" +
            "It is your moment. Make your tool calls, then close in character. You got this, thanks.";

        var response = await _baseAgent.InvokeAgentAsync(ct);

        // Capture the in-character closing response as a Narration entry
        var closingThought = response?.GetText()?.Trim();
        if (!string.IsNullOrWhiteSpace(closingThought)) {
          await _mediator.Send(new AddNarrationToPerformanceCommand(
              performanceId,
              $"{entry.Name}",
              closingThought
          ), ct);
        }
      }
    }
  }
}
