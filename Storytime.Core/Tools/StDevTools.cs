using System.ComponentModel;
using MCPSharp;
using Storytime.Core.Service;
using Storytime.Core.Constants;

namespace Storytime.Core.Tools {
  public class StDevTools {
    private static IStDevToolsHandler GetTools() => DiBridgeService.GetService<IStDevToolsHandler>();
    
    [McpTool(Cx.CmdAddProjectStory, "Adds a new story to a project, returns the project with the new story item.")]
    public static async Task<string> AddStoryToProject(
      [Description("Id of the parent project item")]
      int projectId,
      [Description("Name of the new story")]
      string name,
      [Description("Details of the new story")]
      string details
    ) => await GetTools().AddStoryToProject(projectId, name, details);


    [McpTool(Cx.CmdAddStoryScene, "Adds a new scene to a story, requires: storyId, name, details parameters.")]
    public static async Task<string> AddSceneToStory(
      [Description("Id of the parent story item")]
      int storyId,
      [Description("Name of the new scene")]
      string name,
      [Description("Details of the new scene")]
      string details
    ) => await GetTools().AddSceneToStory(storyId, name, details);


    [McpTool(Cx.CmdAddSceneBeat, "Adds a new beat to a scene, requires: sceneId, name, details parameters.")]
    public static async Task<string> AddBeatToScene(
      [Description("Id of the parent scene item")]
      int sceneId,
      [Description("Name of the new beat")]
      string name,
      [Description("Details of the new beat")]
      string details
    ) => await GetTools().AddBeatToScene(sceneId, name, details);

    [McpTool(Cx.CmdAddStoryCharacter, "Adds a new character to a story, requires: storyId, name, details parameters.")]
    public static async Task<string> AddCharacterToStory(
      [Description("Id of the parent story or scene item")]
      int storyId,
      [Description("Name of the new character")]
      string name,
      [Description("Details of the new character")]
      string details
    ) => await GetTools().AddCharacterToStory(storyId, name, details);


    [McpTool(Cx.CmdAddCallSheetNarration, "Adds a new narration to a call sheet, requires: callSheetId, name, details parameters.")]
    public static async Task<string> AddNarrationToCallSheet(
      [Description("Id of the parent call sheet item")]
      int callSheetId,
      [Description("Name of the new narration")]
      string name,
      [Description("Details of the new narration")]
      string details
    ) => await GetTools().AddNarrationToCallSheet(callSheetId, name, details);

    [McpTool(Cx.CmdAddCallSheetRole, "Adds a new role to a call sheet, requires: callSheetId, characterId, name, details parameters.")]
    public static async Task<string> AddRoleToCallSheet(
      [Description("Id of the parent call sheet item")]
      int callSheetId,
      [Description("Id of the character for the new role")]
      int characterId,
      [Description("Name of the new role")]
      string name,
      [Description("Details of the new role")]
      string details
    ) => await GetTools().AddRoleToCallSheet(callSheetId, characterId, name, details);


  }
}
