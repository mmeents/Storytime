using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Storytime.Core.Handlers.AsGraph;
using Storytime.Core.Constants;

namespace Storytime.Core.Agents {
  public interface IStoryWriterAgent {
    Task GenerateSceneAndCharacterForStory(int storyId, CancellationToken ct = default);
  }
  public class StoryWriterAgent : IStoryWriterAgent {
    private readonly ILocalBaseAgent _baseAgent;
    private readonly IMediator _mediator;    
    public StoryWriterAgent(ILocalBaseAgentFactory baseAgentFactory, IMediator mediator, StorytimeDbContext context) {
      _baseAgent = baseAgentFactory.Create();
      _baseAgent.ToolsToUse.Add(Cx.LMStudioStorytimeMcpToolName);
      _baseAgent.Name = $"Sophie the Story Writer at {Cx.AppName}";
      _mediator = mediator;      
    }
    public async Task GenerateSceneAndCharacterForStory(int storyId, CancellationToken ct = default) {
      var story = await _mediator.Send(new GetSubgraphQuery(storyId, 1), ct);

      var existingScenes = story.Nodes
          .Where(n => n.Item.ItemTypeId == (int)StItemType.Scene)
          .Select(n => $" {n.Item.Id}: {n.Item.Name},");


      var existingChars = story.Nodes
          .Where(n => n.Item.ItemTypeId == (int)StItemType.Character)
          .Select(n => $" {n.Item.Id}: {n.Item.Name},");

      _baseAgent.SystemPrompt =
          $"You are the Creative Story Writer."+Environment.NewLine +
           "You are to create at least one new unique scene. Optionally add characters if not there on the story. " + Environment.NewLine +                    
          $"Use {Cx.CmdAddScene} mcp tool to persist the scene. Use {Cx.CmdAddCharacter} mcp tool to persist the characters. {Cx.CmdGetById} to retreive an item. "+
          "Important, within the tool call parameters, no curly brackets or JSON characters, they will break calls. thanks";

      _baseAgent.UserPrompt =
          $"The story, we need a new, unique, scene. Maybe characters if needed, is: \n" +
          $"Story (Id:{storyId}): {story.Root.Name}\n" +
          $"{story.Root.Description}\n\n" +
          $"Existing Scenes(whats been done):\n{string.Join("\n", existingScenes).Trim()}\n" +
          $"Existing Cast:\n{string.Join("\n", existingChars).Trim()}\n\n" +        
          $"Use the {Cx.LMStudioStorytimeMcpToolName} mcp server, tools there are for you. You got this, thanks.";
      await _baseAgent.InvokeAgentAsync(ct);
    }
  }
}
