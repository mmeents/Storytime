using MediatR;
using Microsoft.Identity.Client;
using Storytime.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storytime.Core.Handlers.AsGraph;
using Microsoft.Extensions.Logging;

namespace Storytime.Core.Agents {
  public interface IDevelopmentManagerAgent {
    Task GenerateStoryIdeaForProject(int projectId, CancellationToken cancellationToken);
  }

  public class DevelopmentManagerAgent : IDevelopmentManagerAgent {
    private readonly ILocalBaseAgent _baseAgent;
    private readonly ILogger<DevelopmentManagerAgent> _logger;
    private readonly IMediator _mediator;


    public DevelopmentManagerAgent(ILocalBaseAgentFactory baseAgentFactory, ILogger<DevelopmentManagerAgent> logger, IMediator mediator) {
      _baseAgent = baseAgentFactory.Create();
      if (_baseAgent.ToolsToUse.Count > 0) _baseAgent.ToolsToUse.Clear();
      _baseAgent.ToolsToUse.Add(Cx.LMStudioStorytimeMcpToolName);
      _baseAgent.Name = $"Dave the Development Manager at {Cx.AppName}";
      _logger = logger;
      _mediator = mediator; 
    }


    public async Task GenerateStoryIdeaForProject(int projectId, CancellationToken cancellationToken) {

      var project = await _mediator.Send(new GetSubgraphQuery(projectId, 1), cancellationToken);
      if (project == null || project.Root == null) {
        _logger.LogError("Project with id {projectId} not found.", projectId);
        throw new Exception($"Project with id {projectId} not found.");
      }

      try {

        var projectName = project?.Root?.Name ?? "hmm, missing name?";
        var projectDescription = project?.Root?.Description ?? "hmm, missing description?";
        var existingStoryIdeas = project!.Nodes.Where(n => n.Item.ItemTypeId == (int)StItemType.Story).Select(n => n.Item).ToList();
        var existingStoryIdeasText = existingStoryIdeas.Count > 0 ? string.Join(Environment.NewLine, existingStoryIdeas.Select(s => $"id:{s.Id} "+ s.Name)) : "No existing story ideas.";

        _baseAgent.SystemPrompt = $"You are Dave, the Development Manager at {Cx.AppName}. " + Environment.NewLine+
          $"Your role is to generate compelling, unique story ideas that align with a project's theme and tone. "+ Environment.NewLine +
          "Attach it to the organization's knowledge " +
          $"graph with the {Cx.LMStudioStorytimeMcpToolName} {Cx.CmdAddProjectStory} tool.";      

        _baseAgent.UserPrompt = $"Generate 1 new unique story idea for project with id {projectId}: "+
          $"Project Name: {projectName}" + Environment.NewLine +
          $"Project Description: {projectDescription}" + Environment.NewLine + 
          $"Existing Stories (do not duplicate thse): {existingStoryIdeasText}"+Environment.NewLine +        
          $"And finally, Using the {Cx.LMStudioStorytimeMcpToolName} {Cx.CmdAddProjectStory} tool, be sure to pass all 3 parameters: projectId, name, details; they are required. you got this...";

      
        var response = await _baseAgent.InvokeAgentAsync(cancellationToken);

      } 
      catch (Exception ex) {      
        _logger.LogError(ex, "An error occurred while generating a story idea for project id {projectId}: {errorMessage}", projectId, ex.Message);
      }
    }
  }
}
