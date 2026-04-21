using KB.Core.Entities;
using KB.Core.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Storytime.Core.Constants;
using Storytime.Core.Handlers.AsGraph;
using Storytime.Core.Handlers.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storytime.Core.Agents {
  public interface ISceneWriterAgent {
    Task GenerateBeatsForScene(int storyId, int sceneId, CancellationToken ct = default);
  }

  public class SceneWriterAgent : ISceneWriterAgent {
    private readonly ILocalBaseAgent _baseAgent;
    private readonly IMediator _mediator;
    private readonly ILogger<SceneWriterAgent> _logger;

    public SceneWriterAgent(ILocalBaseAgentFactory baseAgentFactory, IMediator mediator, ILogger<SceneWriterAgent> logger) {
      _baseAgent = baseAgentFactory.Create();
      if (_baseAgent.ToolsToUse.Count >0) _baseAgent.ToolsToUse.Clear();
      _baseAgent.ToolsToUse.Add(Cx.LMStudioStorytimeMcpToolName);
      _baseAgent.Name = $"Blake the Scene Writer at {Cx.AppName}";
      _mediator = mediator;
      _logger = logger;
    }

    public async Task GenerateBeatsForScene(int storyId, int sceneId, CancellationToken ct = default) {
      var newSceneId = sceneId;
      var scene = await _mediator.Send(new GetSubgraphQuery(sceneId, 1), ct);
      if (scene.Root == null) { 
        _logger.LogError("Scene writer set says no scene.  {sceneId}", sceneId);
        throw new Exception($"Scene with id {sceneId} not found.");
      }
      try { 

        if (scene.Nodes.Any(n => n.Item?.ItemTypeId == (int)StItemType.Beat)) {        
          var relItem = new CreateRelatedItemCommand(storyId,(int)StRelationType.Contains,(int)StItemType.Scene,    
            scene.Root.Name, scene.Root.Description, scene.Root.Data);        
          var newScene = await _mediator.Send(relItem, ct);
          newSceneId = newScene!.Id;
        }      

        _baseAgent.SystemPrompt =
            $"You are the Scene Writer at {Cx.AppName}." +Environment.NewLine +
            "Your job is to Create 3–6 new Beats that tell this scene moment-by-moment. " + Environment.NewLine +
            "Each beat should have a clear purpose (Setup / Choice / Escalation / Climax / Resolution)." + Environment.NewLine +
            $"Use the {Cx.CmdAddSceneBeat} mcp tool to persist each beat." + Environment.NewLine +
            "Important, within the tool call parameters, no curly brackets or JSON characters. Thanks.";

        _baseAgent.UserPrompt =
            $"Scene (Id:{newSceneId}): {scene.Root.Name}\n" +
            $"{scene.Root.Description}\n\n" +
            $"Generate the new unique beats for this scene using the {Cx.LMStudioStorytimeMcpToolName} server. " +
            "You got this, thanks.";

        await _baseAgent.InvokeAgentAsync(ct);

      } catch (Exception ex) {
          _logger.LogError(ex, "Scene writer agent failed for scene {sceneId}", sceneId);
          throw;
      }
    }
  }
}
