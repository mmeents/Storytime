using MediatR;
using Storytime.Core.Handlers.Agents;
using Storytime.Core.Handlers.AsGraph;
using Storytime.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storytime.Core.Agents {
  public interface IDirectorAgent {
    Task DirectScene( int storyId, int sceneId, CancellationToken ct = default);
  }

  public class DirectorAgent : IDirectorAgent {
    private readonly ILocalBaseAgent _baseAgent;
    private readonly IMediator _mediator;

    public DirectorAgent(ILocalBaseAgentFactory baseAgentFactory, IMediator mediator) {
      _baseAgent = baseAgentFactory.Create();
      if (_baseAgent.ToolsToUse.Count > 0) _baseAgent.ToolsToUse.Clear();
      _baseAgent.ToolsToUse.Add(Cx.LMStudioStorytimeMcpToolName);
      _baseAgent.Name = $"Dean the Director at {Cx.AppName}";
      _mediator = mediator;
    }

    public async Task DirectScene(int storyId, int sceneId, CancellationToken ct = default) {

      // Pre-load scene with its beats
      var scene = await _mediator.Send(new GetSubgraphQuery(sceneId, 1), ct);
      if (scene.Root == null)
        throw new Exception($"Scene with id {sceneId} not found.");

      // Pre-load story for available characters
      var story = await _mediator.Send(new GetSubgraphQuery(storyId, 1), ct);
      if (story.Root == null)
        throw new Exception($"Story with id {storyId} not found.");

      var beats = scene.Nodes
          .Where(n => n.Item.ItemTypeId == (int)StItemType.Beat)
          .OrderBy(n => n.Relation.Rank ?? n.Relation.Id)
          .Select(n => $"  - {n.Item.Name}: {n.Item.Description}");

      var characters = story.Nodes
          .Where(n => n.Item.ItemTypeId == (int)StItemType.Character)
          .Select(n => $"  - CharacterId:{n.Item.Id} | {n.Item.Name}: {n.Item.Description}");

      // Add call sheet was modified to be different and return call sheet.
      var callSheetDto = await _mediator.Send(
        new AddCallSheetToSceneCommand(
          sceneId,
          $"{scene.Root.Name} - Call Sheet",
          $"Director's call sheet for: {scene.Root.Name}"
        ), ct);

      if (callSheetDto == null)
        throw new Exception($"Failed to create call sheet for scene {sceneId}.");
      var callSheetId = callSheetDto.Id;

      _baseAgent.SystemPrompt =
          $"You are Dean the Director at {Cx.AppName}." + Environment.NewLine +
          $"Your job is to cast and narrate Scene (Id:{sceneId}): {scene.Root.Name}." + Environment.NewLine +
          $"The Call Sheet has already been created. Call Sheet Id: {callSheetId}." + Environment.NewLine +
          "Build the call sheet in the exact dramatic order the scene should play out." + Environment.NewLine +
          "Use these two tools to do it:" + Environment.NewLine +
          $"  {Cx.CmdAddRoleToCallSheet} - cast a character with their acting instruction." + Environment.NewLine +
          $"  {Cx.CmdAddNarrationToCallSheet} - add a narration beat between character moments." + Environment.NewLine +
          "Interleave narration and roles naturally — the order of your calls is the order of playback." + Environment.NewLine +
          "Important: no curly brackets or JSON characters in tool call parameters. Thanks.";

      _baseAgent.UserPrompt =
          $"Scene (Id:{sceneId}): {scene.Root.Name}" + Environment.NewLine +
          $"{scene.Root.Description}" + Environment.NewLine + Environment.NewLine +
          $"Beats (in order):{Environment.NewLine}{string.Join(Environment.NewLine, beats)}" + Environment.NewLine + Environment.NewLine +
          $"Available Characters:{Environment.NewLine}{string.Join(Environment.NewLine, characters)}" + Environment.NewLine + Environment.NewLine +
          $"Your Call Sheet Id is: {callSheetId}" + Environment.NewLine + Environment.NewLine +
          $"Cast and narrate this scene using the {Cx.LMStudioStorytimeMcpToolName} server. " + Environment.NewLine +
          "Interleave narration and character roles in the order they should play out. You got this, thanks.";

      await _baseAgent.InvokeAgentAsync(ct);
    }
  }
}
