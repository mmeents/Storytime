using Microsoft.Extensions.DependencyInjection;
using Storytime.Core.Agents;
using Storytime.Core.Constants;
using Storytime.Core.Entities;

namespace Storytime.Core.Service {
  public interface IFactorySettingsService {
    AgentRunnerMode CurrentMode { get; set; }
    string LMStudioUrl { get; set; }
    string LMStudioApiKey { get; set; }
    string LMStudioModel { get; set; }
    string ClaudeModel { get; set; }
    string ClaudeLaunchPath { get; set; }
    string StorytimeExportPath { get; set; }
    string StorytimeLogsPath { get; set; }
    void Save();
  }


  public class FactorySettingsService : IFactorySettingsService {
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public FactorySettingsService(
      IServiceScopeFactory serviceScopeFactory
    
    ) {
      _serviceScopeFactory = serviceScopeFactory;      
      CurrentMode = Enum.TryParse(getValue(nameof(CurrentMode), Cx.DefaultAgentRunnerMode.ToString()), out AgentRunnerMode mode) ? mode : Cx.DefaultAgentRunnerMode;
      LMStudioUrl = getValue(nameof(LMStudioUrl), Cx.LMStudioUrl);
      LMStudioApiKey = getValue(nameof(LMStudioApiKey), Cx.LMStudioApiKey);
      LMStudioModel = getValue(nameof(LMStudioModel), Cx.LMStudioDefaultModel);
      ClaudeModel = getValue(nameof(ClaudeModel), Cx.ClaudeDefaultModel);
      ClaudeLaunchPath = getValue(nameof(ClaudeLaunchPath), Cx.ClaudeExecutablePath);
      StorytimeExportPath = getValue(nameof(StorytimeExportPath), Cx.ExportPath);
      StorytimeLogsPath = getValue(nameof(StorytimeLogsPath), Cx.LogsAppPath);
    }

    private AgentRunnerMode _currentMode = Cx.DefaultAgentRunnerMode;
    public AgentRunnerMode CurrentMode { 
      get => _currentMode;
      set { 
        _currentMode = value;
        setValue(nameof(CurrentMode), value.ToString());
      }
    }

    private string _lmStudioModel = Cx.LMStudioDefaultModel;
    public string LMStudioModel { 
      get => _lmStudioModel;
      set { 
        _lmStudioModel = value;
        setValue(nameof(LMStudioModel), value);
      }
    }

    private string _lmStudioUrl = Cx.LMStudioUrl;
    public string LMStudioUrl { 
      get => _lmStudioUrl;
      set {
        _lmStudioUrl = value;
        setValue(nameof(LMStudioUrl), value);
      }
    }

    private string _lmStudioApiKey = Cx.LMStudioApiKey;
    public string LMStudioApiKey { 
      get => _lmStudioApiKey;
      set {
        _lmStudioApiKey = value;
        setValue(nameof(LMStudioApiKey), value);
      }
    }

    private string _claudeModel = Cx.ClaudeDefaultModel;
    public string ClaudeModel { 
      get => _claudeModel;
      set {
        _claudeModel = value;
        setValue(nameof(ClaudeModel), value);
      }
    }

    private string _claudeLaunchPath = Cx.ClaudeExecutablePath;
    public string ClaudeLaunchPath { 
      get => _claudeLaunchPath;
      set {
        _claudeLaunchPath = value;
        setValue(nameof(ClaudeLaunchPath), value);
      }
    }

    private string _storytimeExportPath = Cx.ExportPath;
    public string StorytimeExportPath { 
      get => _storytimeExportPath;
      set {
        _storytimeExportPath = value;
        setValue(nameof(StorytimeExportPath), value);
      }
    }

    private string _storytimeLogsPath = Cx.LogsAppPath;
    public string StorytimeLogsPath { 
      get => _storytimeLogsPath;
      set {
        _storytimeLogsPath = value;
        setValue(nameof(StorytimeLogsPath), value);
      }
    }


    public string getValue(string key, string defaultValue) {
      using (var scope = _serviceScopeFactory.CreateScope()) {
        var context = scope.ServiceProvider.GetRequiredService<StorytimeDbContext>();
        var aSetting = context.GetAppSetting(key);
        if (aSetting == null) {        
          return defaultValue;
        } else {
          return aSetting?.Value ?? defaultValue;
        }
      }
    }

    public void setValue(string key, string value) {
      if (String.IsNullOrEmpty(key)) throw new ArgumentException("Key cannot be null or empty", nameof(key));
      using (var scope = _serviceScopeFactory.CreateScope()) {
        var context = scope.ServiceProvider.GetRequiredService<StorytimeDbContext>();
        var aSetting = context.GetAppSetting(key);
        if (aSetting == null) {
          var bSetting = new AppSetting { Key = key, Value = value, ValueInt = 0 };
          var _ = context.SetAppSetting(bSetting);
        } else {
          aSetting.Value = value;
          var _ = context.SetAppSetting(aSetting); 
        }
      }
    }   

    public void Save() {
      setValue(nameof(CurrentMode), CurrentMode.ToString());
      setValue(nameof(LMStudioUrl), LMStudioUrl);
      setValue(nameof(LMStudioApiKey), LMStudioApiKey);
      setValue(nameof(LMStudioModel), LMStudioModel);
      setValue(nameof(ClaudeModel), ClaudeModel);
      setValue(nameof(ClaudeLaunchPath), ClaudeLaunchPath);
      setValue(nameof(StorytimeExportPath), StorytimeExportPath);
      setValue(nameof(StorytimeLogsPath), StorytimeLogsPath);
    }
  }
}
