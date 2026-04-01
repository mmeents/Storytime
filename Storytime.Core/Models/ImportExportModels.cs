using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KB.Core.Models;

namespace Storytime.Core.Models {
  public class ImportExportModels {
    public List<ItemTypeDto> ItemTypes { get; set; } = new();
    public List<ItemRelationTypeDto> ItemRelationTypes { get; set; } = new();
    public List<ItemDto> Items { get; set; } = new();
    public List<ItemRelationDto> ItemRelations { get; set; } = new();

    public ImportExportModels() { }

  }
}
