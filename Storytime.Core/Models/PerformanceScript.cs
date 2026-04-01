using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storytime.Core.Models {
  public class PerformanceEntry {
    public int Rank { get; set; }
    public string Type { get; set; } = "Speech"; // "Speech" | "Action" | "Narration"
    public int? CharacterId { get; set; }
    public string CharacterName { get; set; } = "";
    public string Text { get; set; } = "";
  }

  public class PerformanceScript {
    public List<PerformanceEntry> Entries { get; set; } = new();
  }
}
