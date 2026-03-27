using MCPSharp;
using Storytime.Core.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storytime.Core.Tools {
  public class StorytimeTools {
    private static IStorytimeToolsHandler GetTools() => DiBridgeService.GetService<IStorytimeToolsHandler>();

    [McpTool("get-projects", "Gets all items of type project, they are the root items. ")]
    public static async Task<string> GetProjects() => await GetTools().GetProjectItems();

    [McpTool("get-item-by-id", "Gets an item by Id")]
    public static async Task<string> GetItemById(
      [Description("the Item Id to get")] int id 
    ) => await GetTools().GetItemById(id);

    [McpTool("create-item", "create new item")]
    public static async Task<string> CreateItem(
      [Description("Item's name")]
      string name, 
      [Description("valid(itemTypeId:name) types: 1:Project, 2:Story, 3:Scene, 4:Beat, 5:Character, 6:Location, 7:Rule")]
      int itemTypeId, 
      [Description("Item's Description")]
      string description = "", 
      [Description("Item's JSON data")]
      string dataJson = "{}"
    ) => await GetTools().CreateItem(name, itemTypeId, description, dataJson);


    [McpTool("update-item", "update existing item")]
    public static async Task<string> UpdateItem(
      [Description("Item's ID")]
      int id,
      [Description("Item's name")]
      string name, 
      [Description("valid(itemTypeId:name) types: 1:Project, 2:Story, 3:Scene, 4:Beat, 5:Character, 6:Location, 7:Rule")]
      int itemTypeId, 
      [Description("Item's Description")]
      string description = "", 
      [Description("Item's JSON data")]
      string dataJson = "{}"
    ) => await GetTools().UpdateItem(id, name, itemTypeId, description, dataJson);

    [McpTool("get-relation-by-id", "Gets a relation by id")]
    public static async Task<string> GetRelationById(int id
    ) => await GetTools().GetRelationById(id);

    [McpTool("create-relation", "create a relation")]
    public static async Task<string> CreateRelation(
      [Description("Id of the source Item")]
      int fromItemId, 
      [Description("Id of the target Item")]
      int toItemId, 
      [Description("valid (Id:type) are: 1:Contains, 2:HasBeat, 3:NextBeat, 4:UsesRule, 5:FeaturesCharacter, 6:TakesPlaceAt")]
      int relationTypeId       
    ) => await GetTools().CreateRelation(fromItemId, toItemId, relationTypeId);

    [McpTool("update-relation", "update a relation")]
    public static async Task<string> UpdateRelation(
      [Description("Id of the Relation to update")]
      int relationId, 
      [Description("Id of the source Item")]
      int fromItemId, 
      [Description("Id of the target Item")]
      int toItemId, 
      [Description("valid (Id:type) are: 1:Contains, 2:HasBeat, 3:NextBeat, 4:UsesRule, 5:FeaturesCharacter, 6:TakesPlaceAt")]
      int relationTypeId       
    ) => await GetTools().UpdateRelation(relationId, fromItemId, toItemId, relationTypeId);

    [McpTool("get-subgraph", "Gets the item and related out to depth levels deep.")]
    public static async Task<string> GetSubgraph(
      [Description("the Item Id to start from")] int itemId, 
      [Description("how many levels of relations to include?")] int depth = 3
    ) => await GetTools().GetSubgraph(itemId, depth);

  }
}
