using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using MCPSharp;
using Storytime.Core.Service;
using Storytime.Core.Constants;

namespace Storytime.Core.Tools {
  public class StDevTools {
    private static IStDevToolsHandler GetTools() => DiBridgeService.GetService<IStDevToolsHandler>();

    [McpTool(Cx.CmdAddCharacter, "Adds a new character to a story or scene, returns the story or scene with new character.")]
    public static async Task<string> AddCharacterToStory(
      [Description("Id of the parent story or scene item")]
      int storyId,
      [Description("Name of the new character")]
      string name,
      [Description("Description of the new character")]
      string description = ""
    ) => await GetTools().AddCharacterToStory(storyId, name, description);


    [McpTool(Cx.CmdAddStory, "Adds a new story to a project, returns the project with the new story item.")]
    public static async Task<string> AddStoryToProject(
      [Description("Id of the parent project item")]
      int projectId,
      [Description("Name of the new story")]
      string name,
      [Description("Description of the new story")]
      string description = ""
    ) => await GetTools().AddStoryToProject(projectId, name, description);


    [McpTool(Cx.CmdAddScene, "Adds a new scene to a story, returns the story with the new scene item.")]
    public static async Task<string> AddSceneToStory(
      [Description("Id of the parent story item")]
      int storyId,
      [Description("Name of the new scene")]
      string name,
      [Description("Description of the new scene")]
      string description = ""
    ) => await GetTools().AddSceneToStory(storyId, name, description);


    [McpTool(Cx.CmdAddBeat, "Adds a new beat to a scene, returns the scene with the new beat item.")]
    public static async Task<string> AddBeatToScene(
      [Description("Id of the parent scene item")]
      int sceneId,
      [Description("Name of the new beat")]
      string name,
      [Description("Description of the new beat")]
      string description = ""
    ) => await GetTools().AddBeatToScene(sceneId, name, description);

    [McpTool(Cx.CmdAddNarrationToCallSheet, "Adds a new narration to a call sheet, returns the call sheet with the new narration item.")]
    public static async Task<string> AddNarrationToCallSheet(
      [Description("Id of the parent call sheet item")]
      int callSheetId,
      [Description("Name of the new narration")]
      string name,
      [Description("Description of the new narration")]
      string description = ""
    ) => await GetTools().AddNarrationToCallSheet(callSheetId, name, description);

    [McpTool(Cx.CmdAddRoleToCallSheet, "Adds a new role to a call sheet, returns the call sheet with the new role item.")]
    public static async Task<string> AddRoleToCallSheet(
      [Description("Id of the parent call sheet item")]
      int callSheetId,
      [Description("Id of the character for the new role")]
      int characterId,
      [Description("Name of the new role")]
      string name,
      [Description("Description of the new role")]
      string description = ""
    ) => await GetTools().AddRoleToCallSheet(callSheetId, characterId, name, description);


  }
}
