using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Storytime.Core.Handlers.AsGraph;
using Storytime.Core.Constants;
using Microsoft.Extensions.Logging;

namespace Storytime.Core.Agents {
  public interface IStoryWriterAgent {
    Task GenerateSceneAndCharacterForStory(int storyId, CancellationToken ct = default);
  }
  public class StoryWriterAgent : IStoryWriterAgent {
    private readonly ILocalBaseAgent _baseAgent;
    private readonly IMediator _mediator; 
    private readonly ILogger<StoryWriterAgent> _logger;
    public StoryWriterAgent(ILocalBaseAgentFactory baseAgentFactory, IMediator mediator, ILogger<StoryWriterAgent> logger, StorytimeDbContext context) {
      _baseAgent = baseAgentFactory.Create();
      if (_baseAgent.ToolsToUse.Count > 0) _baseAgent.ToolsToUse.Clear();
      _baseAgent.ToolsToUse.Add(Cx.LMStudioStorytimeMcpToolName);
      _baseAgent.Name = $"Sophie the Story Writer at {Cx.AppName}";
      _mediator = mediator;      
      _logger = logger;
    }
    public async Task GenerateSceneAndCharacterForStory(int storyId, CancellationToken ct = default) {
      try {
      
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
            $"Use {Cx.CmdAddStoryScene} mcp tool to persist the scene. Use {Cx.CmdAddStoryCharacter} mcp tool to persist the characters. {Cx.CmdGetById} to retreive an item. {Cx.CmdGetHelp} can be used. "+
             "Important, within the tool call parameters, no curly brackets or JSON characters, they will break calls. thanks";

        _baseAgent.UserPrompt =
            $"The story, we need a new, unique, scene. Maybe characters if needed, is: \n" +
            $"Story (Id:{storyId}): {story.Root.Name}\n" +
            $"{story.Root.Description}\n\n" +
            $"Existing Scenes(whats been done):\n{string.Join("\n", existingScenes).Trim()}\n" +
            $"Existing Cast:\n{string.Join("\n", existingChars).Trim()}\n\n" +        
            $"Use the {Cx.LMStudioStorytimeMcpToolName} mcp server, tools there are for you. You got this, thanks.";
        await _baseAgent.InvokeAgentAsync(ct);

      } catch (Exception ex) {
        _logger.LogError(ex, "Error in GenerateSceneAndCharacterForStory for storyId {storyId}",storyId);
      }
    }
  }
}
