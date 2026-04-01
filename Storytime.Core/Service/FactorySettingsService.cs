using Storytime.Core.Agents;
using Storytime.Core.Constants;

namespace Storytime.Core.Service {
  public interface IFactorySettingsService {
    AgentRunnerMode CurrentMode { get; set; }
    string LMStudioModel { get; set; }
    string ClaudeModel { get; set; }
  }


  public class FactorySettingsService : IFactorySettingsService {

    public FactorySettingsService() {
    }

    public AgentRunnerMode CurrentMode { get; set; } = Cx.DefaultAgentRunnerMode;
    public string LMStudioModel { get; set; } = Cx.LMStudioDefaultModel;
    public string ClaudeModel { get; set; } = Cx.ClaudeDefaultModel;

  }
}
