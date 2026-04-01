using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Storytime.Core.Handlers.Agents;
using Storytime.Core.Models;
using System.Text.Json;
using Storytime.Core.Constants;

namespace Storytime.Core.Tools {
  public interface IStDevToolsHandler {
    Task<string> AddStoryToProject(int projectId, string name, string description);
    Task<string> AddSceneToStory(int storyId, string name, string description);
    Task<string> AddBeatToScene(int sceneId, string name, string description);
    Task<string> AddCharacterToStory(int storyId, string name, string description);
    Task<string> AddNarrationToCallSheet(int callSheetId, string section, string narration);
    Task<string> AddRoleToCallSheet(int callSheetId, int characterId, string name, string description);

  }

  public class StDevToolsHandler : IStDevToolsHandler {
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public StDevToolsHandler(IServiceScopeFactory serviceScopeFactory) {
      _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<string> AddStoryToProject(int projectId, string name, string description) {
      using var scope = _serviceScopeFactory.CreateScope();
      var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
      var command = new AddStoryToProjectCommand(projectId, name, description);
      var result = await mediator.Send(command);
      if (result == null) {
        var opResult = McpOpResult.CreateFailure("add-story-to-project", "Failed to add story to project");
        return JsonSerializer.Serialize(opResult);
      } else {
        var opResult = McpOpResult.CreateSuccess("add-story-to-project", "Successfully added story to project", result);
        return JsonSerializer.Serialize(opResult);
      }
    }

    public async Task<string> AddSceneToStory(int storyId, string name, string description) {
      using var scope = _serviceScopeFactory.CreateScope();
      var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
      var command = new AddSceneToStoryCommand(storyId, name, description);
      var result = await mediator.Send(command);
      if (result == null) {
        var opResult = McpOpResult.CreateFailure("add-scene-to-story", "Failed to add scene to story");
        return JsonSerializer.Serialize(opResult);
      } else {
        var opResult = McpOpResult.CreateSuccess("add-scene-to-story", "Successfully added scene to story", result);
        return JsonSerializer.Serialize(opResult);
      }
    }

    public async Task<string> AddBeatToScene(int sceneId, string name, string description) {
      using var scope = _serviceScopeFactory.CreateScope();
      var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
      var command = new AddBeatToSceneCommand(sceneId, name, description);
      var result = await mediator.Send(command);
      if (result == null) {
        var opResult = McpOpResult.CreateFailure("add-beat-to-scene", "Failed to add beat to scene");
        return JsonSerializer.Serialize(opResult);
      } else {
        var opResult = McpOpResult.CreateSuccess("add-beat-to-scene", "Successfully added beat to scene", result);
        return JsonSerializer.Serialize(opResult);
      }
    }

    public async Task<string> AddCharacterToStory(int storyId, string name, string description) {
      using var scope = _serviceScopeFactory.CreateScope();
      var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
      var command = new AddCharacterToStoryCommand(storyId, name, description);
      var result = await mediator.Send(command);
      if (result == null) {
        var opResult = McpOpResult.CreateFailure("add-character-to-story", "Failed to add character to story");
        return JsonSerializer.Serialize(opResult);
      } else {
        var opResult = McpOpResult.CreateSuccess("add-character-to-story", "Successfully added character to story", result);
        return JsonSerializer.Serialize(opResult);
      }
    } 

    public async Task<string> AddNarrationToCallSheet(int callSheetId, string section, string narration) {
      using var scope = _serviceScopeFactory.CreateScope();
      var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
      var command = new AddNarrationToCallSheetCommand(callSheetId, section, narration);
      var result = await mediator.Send(command);
      if (result == null) {
        var opResult = McpOpResult.CreateFailure(Cx.CmdAddNarrationToCallSheet, "Failed to add narration to call sheet");
        return JsonSerializer.Serialize(opResult);
      } else {
        var opResult = McpOpResult.CreateSuccess(Cx.CmdAddNarrationToCallSheet, "Successfully added narration to call sheet", result);
        return JsonSerializer.Serialize(opResult);
      }
    }

    public async Task<string> AddRoleToCallSheet(int callSheetId, int characterId, string name, string description) {
      using var scope = _serviceScopeFactory.CreateScope();
      var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
      var command = new AddRoleToCallSheetCommand(callSheetId, characterId, name, description);
      var result = await mediator.Send(command);
      if (result == null) {
        var opResult = McpOpResult.CreateFailure(Cx.CmdAddRoleToCallSheet, "Failed to add role to call sheet");
        return JsonSerializer.Serialize(opResult);
      } else {
        var opResult = McpOpResult.CreateSuccess(Cx.CmdAddRoleToCallSheet, "Successfully added role to call sheet", result);
        return JsonSerializer.Serialize(opResult);
      }
    }






  }
}
