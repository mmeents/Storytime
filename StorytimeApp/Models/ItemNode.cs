using KB.Core.Models;
using Storytime.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorytimeApp.Models {
  public partial class ItemNode : TreeNode {
    public ItemNode(): base() { }    
    public ItemDto? Item { get; set; } = null;
    public ItemRelationDto? Relation { get; set; } = null;

  }

  public static class ItemNodeExt {

    // special case project.
    public static ItemNode ToItemNode(this ItemDto item) {
      var node = new ItemNode {
        Name = item.Id.ToString(),
        ImageIndex = item.ItemTypeId,
        SelectedImageIndex = item.ItemTypeId,
        Text = item.Name,
        Item = item
      };
      return node;
    }

    // general case 
    public static ItemNode ToItemNode(this ItemRelationDto relation, ItemDto item) {
      var node = new ItemNode {
        Name = item.Id.ToString(),
        ImageIndex = item.ItemTypeId,
        SelectedImageIndex = item.ItemTypeId,
        Text = item.Name,
        Item = item,
        Relation = relation
      };
      return node;
    }
    
    public static ItemNode? FindAncestorOfType(this ItemNode node, StItemType itemType) {
      var current = node.Parent as ItemNode;
      while (current != null) {
        if ( current.Item?.ItemTypeId == (int)itemType)
          return current;
        current = current.Parent as ItemNode;
      }
      return null;
    }
  }

}
