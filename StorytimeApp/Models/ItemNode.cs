using KB.Core.Models;
using Storytime.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorytimeAr.Models {
  public partial class ItemNode : TreeNode {
    public ItemNode(): base() { }
    public bool IsRelationNode { get; set; } = false;
    public ItemDto? Item { get; set; } = null;
    public ItemRelationDto? Relation { get; set; } = null;

  }

  public static class ItemNodeExt {
    public static ItemNode ToItemNode(this ItemDto item) {
      var node = new ItemNode {
        Name = item.Id.ToString(),
        Text = item.Name,
        Item = item,
        IsRelationNode = false
      };
      return node;
    }
    public static ItemNode ToItemNode(this ItemRelationDto relation) {
      var node = new ItemNode {
        Name = relation.Id.ToString(),
        Text = relation.RelationTypeName,
        Relation = relation,
        IsRelationNode = true
      };
      return node;

    }
    public static ItemNode? FindAncestorOfType(this ItemNode node, StItemType itemType) {
      var current = node.Parent as ItemNode;
      while (current != null) {
        if (!current.IsRelationNode && current.Item?.ItemTypeId == (int)itemType)
          return current;
        current = current.Parent as ItemNode;
      }
      return null;
    }
  }

}
