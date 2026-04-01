using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storytime.Core.Models {
  public class ClaudeCliResponse {
    public string Type { get; set; } = string.Empty;
    public string Subtype { get; set; } = string.Empty;
    public bool IsError { get; set; }
    public int DurationMs { get; set; }
    public string Result { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;
    public double TotalCostUsd { get; set; }
    public Usage? Usage { get; set; }
  }

  public class Usage {
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    public int CacheReadInputTokens { get; set; }
    public int CacheCreationInputTokens { get; set; }
  }
}
