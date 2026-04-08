using Azure.Core;
using KB.Core.Entities;
using KB.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Storytime.Core.Entities;
using Storytime.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Storytime.Core.Constants {

  public static class KbExt {

    public static async Task<ItemDto> GetItemDtoById( this StorytimeDbContext context, int Id, CancellationToken cancellationToken = default) {
      var result = await context.Items
        .AsNoTracking()
        .Where(i => i.Id == Id)
        .Select(i => new ItemDto {
          Id = i.Id,
          ItemTypeId = i.ItemTypeId,
          ItemTypeName = i.ItemType.Name,
          Name = i.Name,
          Description = i.Description,
          Data = i.Data,
          Established = i.Established,
          IsActive = i.IsActive,
          Relations = i.Relations.Select(r => new ItemRelationDto {
            Id = r.Id,
            ItemId = r.ItemId,
            ItemName = r.Item != null ? r.Item.Name : "",
            RelatedItemId = r.RelatedItemId,
            RelatedItemName = r.RelatedItem != null ? r.RelatedItem.Name : "",
            RelationTypeId = r.RelationTypeId,
            RelationTypeName = r.RelationType.Relation,
            Rank = r.Rank,
            Established = r.Established,
            RelatedItemHasChildren = r.RelatedItem != null && r.RelatedItem.Relations.Any()
          }).ToList(),
          IncomingRelations = i.IncomingRelations.Select(r => new ItemRelationDto {
            Id = r.Id,
            ItemId = r.ItemId,
            ItemName = r.Item != null ? r.Item.Name : "",
            RelatedItemId = r.RelatedItemId,
            RelationTypeId = r.RelationTypeId,
            RelationTypeName = r.RelationType.Relation,
            Rank = r.Rank,
            Established = r.Established
          }).ToList()
        })
        .FirstOrDefaultAsync(cancellationToken);

      return result ?? throw new Exception($"Item with Id {Id} not found");
    } 

    public static async Task<ItemDto> GetMinimalItemDtoById( this StorytimeDbContext context, int Id, CancellationToken cancellationToken = default) {
      var result = await context.Items
          .AsNoTracking()
          .Where(i => i.Id == Id && i.IsActive)
          .Select(i => new ItemDto {
            Id = i.Id,
            ItemTypeId = i.ItemTypeId,
            ItemTypeName = i.ItemType.Name,
            Name = i.Name,
            Description = i.Description,
            Data = i.Data,
            Established = i.Established,
            IsActive = i.IsActive,   
          })
          .FirstOrDefaultAsync(cancellationToken);

      return result ?? throw new Exception($"Item with Id {Id} not found");
    } 

    public static async Task<int> GetItemsNextRankId(this StorytimeDbContext context, int itemId, CancellationToken cancellationToken = default) {
      var maxRank = await context.ItemRelations
          .Where(ir => ir.ItemId == itemId)
          .MaxAsync(ir => (int?)ir.Rank, cancellationToken);

      return (maxRank ?? 0) + 1;
    }


    public static AppSetting? GetAppSetting(this StorytimeDbContext context, string key) {
      var setting = context.AppSettings.AsNoTracking().Where(s => s.Key == key).FirstOrDefault();
      return setting;

    }

    public static AppSetting? SetAppSetting(this StorytimeDbContext context, AppSetting? value) {
      if (value == null ) {
        return null;
      }
      var setting = context.AppSettings.Where(s => s.Key == value.Key).FirstOrDefault();      
      if (setting == null) {
        setting = new AppSetting { Key = value.Key, Value = value.Value, ValueInt = value.ValueInt };
        context.AppSettings.Add(setting);
      } else {
        setting.Value = value.Value;
        setting.ValueInt = value.ValueInt;
        context.AppSettings.Update(setting);
      }
      context.SaveChanges();
      return setting;

    }
  }
}
