using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storytime.Core.Models {
  public class CharacterPrompt {
    public int Rank { get; set; }
    public string Type { get; set;} = "role"; // role | narration
    public int? CharacterId { get; set; }
    public string Name { get; set; } = "";
    public string Instruction { get; set; } = "";
  }

  public class CallSheetScript {
    public List<CharacterPrompt> Script { get; set; } = new();
  }

}
