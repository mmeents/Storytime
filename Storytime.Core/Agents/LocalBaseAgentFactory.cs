using Microsoft.Extensions.DependencyInjection;
using Storytime.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storytime.Core.Service;

namespace Storytime.Core.Agents {
  // Factory for creating local base agents based on the configured AgentRunnerMode.
  // CopilotCli mode is untested and may require additional setup, but is left here for future expansion.
  public enum AgentRunnerMode { 
    LmStudio
    , ClaudeCode
    //, CopilotCli 
  }

  public interface ILocalBaseAgentFactory {
    ILocalBaseAgent Create();
  }

  public class LocalBaseAgentFactory : ILocalBaseAgentFactory {
    private readonly IServiceProvider _services;
    private readonly IFactorySettingsService _factorySettingsService;

    public LocalBaseAgentFactory(IServiceProvider services, IFactorySettingsService factorySettingsService) {
      _services = services;      
      _factorySettingsService = factorySettingsService;
    }

    public ILocalBaseAgent Create() => _factorySettingsService.CurrentMode switch {
      AgentRunnerMode.ClaudeCode => _services.GetRequiredService<ClaudeCodeBaseAgent>(),
    //  AgentRunnerMode.CopilotCli => _services.GetRequiredService<CopilotCliBaseAgent>(),  untested.
      _ => _services.GetRequiredService<LocalBaseAgent>()
    };
  }



}
