using Newtonsoft.Json.Linq;
using Storytime.Core.Agents;
using Storytime.Core.Constants;
using Storytime.Core.Entities;

namespace Storytime.Core.Service {
  public interface ISettingsService {
    AgentRunnerMode AgentRunnerMode { get; set; }
    string LMStudioUrl { get; set; }
    string LMStudioApiKey { get; set; }
    string LMStudioModel { get; set; }
    string ClaudeModel { get; set; }
    string ClaudeLaunchFolder { get; set; }
    string StorytimeExportFolder { get; set; }
    string StorytimeLogsFolder { get; }
    void Save();
  } 

  public class SettingsService : ISettingsService {

    private readonly StorytimeDbContext _context;

    public SettingsService(StorytimeDbContext context) {
      _context = context;
      LMStudioUrl = getValue(nameof(LMStudioUrl), Cx.LMStudioUrl);
      LMStudioApiKey = getValue(nameof(LMStudioApiKey), Cx.LMStudioApiKey);
      LMStudioModel = getValue(nameof(LMStudioModel), Cx.LMStudioDefaultModel);
      ClaudeModel = getValue(nameof(ClaudeModel), Cx.ClaudeDefaultModel);
      AgentRunnerMode = Enum.TryParse(getValue(nameof(AgentRunnerMode), Cx.DefaultAgentRunnerMode.ToString()), out AgentRunnerMode mode) ? mode : Cx.DefaultAgentRunnerMode;
      ClaudeLaunchFolder = getValue(nameof(ClaudeLaunchFolder), Cx.ClaudeExecutablePath);
    }
   
    public AgentRunnerMode AgentRunnerMode { get; set; } = Cx.DefaultAgentRunnerMode;

    public string LMStudioUrl { get; set; } = Cx.LMStudioUrl;
    public string LMStudioApiKey { get; set; } = Cx.LMStudioApiKey;
    public string LMStudioModel { get; set; } = Cx.LMStudioDefaultModel;

    public string ClaudeModel { get; set; } = Cx.ClaudeDefaultModel;
    public string ClaudeLaunchFolder { get; set; } = Cx.ClaudeExecutablePath;
    public string StorytimeExportFolder { get; set; } = Cx.ExportPath;
    public string StorytimeLogsFolder { get; } = Cx.LogsAppPath; // <-- change in Cx if you need too, it's used outsid di.

    public void Save() {
      setValue(nameof(LMStudioUrl), LMStudioUrl);
      setValue(nameof(LMStudioApiKey), LMStudioApiKey);
      setValue(nameof(LMStudioModel), LMStudioModel);
      setValue(nameof(ClaudeModel), ClaudeModel);
      setValue(nameof(AgentRunnerMode), AgentRunnerMode.ToString());
      setValue(nameof(ClaudeLaunchFolder), ClaudeLaunchFolder);
      setValue(nameof(StorytimeExportFolder), StorytimeExportFolder);
    }

    public string getValue(string key, string defaultValue) {
      var aSetting = _context.GetAppSetting(key);
      if (aSetting == null) {
        var bSetting = new AppSetting { Key = key, Value = defaultValue, ValueInt = 0 };
        var anAppSetting = _context.SetAppSetting(bSetting);
        return anAppSetting?.Value ?? defaultValue;
      } else {
        return aSetting?.Value ?? defaultValue;
      }
    }

    public void setValue(string key, string value) {
      if (String.IsNullOrEmpty(key)) throw new ArgumentException("Key cannot be null or empty", nameof(key));
      var aSetting = _context.GetAppSetting(key);
      if (aSetting == null) {
        var bSetting = new AppSetting { Key = key, Value = value, ValueInt = 0 };
        var _ = _context.SetAppSetting(bSetting);
      } else {
        aSetting.Value = value;
        var _ = _context.SetAppSetting(aSetting);
      }
    }

  }
}
