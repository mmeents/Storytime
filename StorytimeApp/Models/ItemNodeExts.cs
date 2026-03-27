using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KB.Core.Models;


namespace StorytimeAr.Models {
  public static class ItemNodeExts {
    public static ItemDto Clone(this ItemDto item) {
      return new ItemDto {
        Id = item.Id,
        ItemTypeId = item.ItemTypeId,
        ItemTypeName = item.ItemTypeName,
        Name = item.Name,
        Description = item.Description,
        Data = item.Data,
        Established = item.Established,
        IsActive = item.IsActive,
        Relations = item.Relations.Select(r => new ItemRelationDto {
          Id = r.Id,
          ItemId = r.ItemId,
          RelatedItemId = r.RelatedItemId,
          RelationTypeId = r.RelationTypeId,
          RelationTypeName = r.RelationTypeName
        }).ToList(),
        IncomingRelations = item.IncomingRelations.Select(r => new ItemRelationDto {
          Id = r.Id,
          ItemId = r.ItemId,
          RelatedItemId = r.RelatedItemId,
          RelationTypeId = r.RelationTypeId,
          RelationTypeName = r.RelationTypeName
        }).ToList()
      };
    } 

    public static ItemRelationDto Clone(this ItemRelationDto itemRelation) {
      return new ItemRelationDto {
        Id = itemRelation.Id,
        ItemId = itemRelation.ItemId,
        RelatedItemId = itemRelation.RelatedItemId,
        RelationTypeId = itemRelation.RelationTypeId,
        RelationTypeName = itemRelation.RelationTypeName
      };
    }
  }
}
