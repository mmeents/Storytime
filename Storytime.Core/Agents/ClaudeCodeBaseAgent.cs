using Storytime.Core.Entities;
using Storytime.Core.Models;
using Storytime.Core.Constants;
using Storytime.Core.Service;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Storytime.Core.Agents {
  public class ClaudeCodeBaseAgent : ILocalBaseAgent {
    private readonly StorytimeDbContext _context;
    private readonly IFactorySettingsService _factorySettingsService;
    private readonly ILogger<ClaudeCodeBaseAgent> _logger;
    public ClaudeCodeBaseAgent(StorytimeDbContext context, IFactorySettingsService factorySettingsService, ILogger<ClaudeCodeBaseAgent> logger) {
      _context = context;
      _factorySettingsService = factorySettingsService;
      _logger = logger;
      Model = _factorySettingsService.ClaudeModel;
    }

    public AgentStatus Status { get; private set; } = AgentStatus.Idle;
    public double Temperature { get; set; } = 0.8;
    public string Model { get; set; }
    public string Name { get; set; } = "";
    public string SystemPrompt { get; set; } = "";
    public string UserPrompt { get; set; } = "";
    public List<string> ToolsToUse { get; set; } = new();  // relec from base and lm studio client.
      // for now we will assume the tools are always available from .mcp.json config file and just
      // confirm the location to the compiled StorytimeMCP.exe is accurate.
    public ChatRequest? LastRequest { get; set; }
    public ChatResponse? LastResponse { get; set; }
    public List<ChatRequest> ChatRequests { get; set; } = new();
    public List<ChatResponse> ChatResponses { get; set; } = new();

    public async Task<ChatResponse> InvokeAgentAsync(CancellationToken cancellationToken) {
      
      if (Status == AgentStatus.Running) {
        _logger.LogError("ClaudeCodeBaseAgent status is running.");
        throw new InvalidOperationException("Agent is already running.");
      }

      Status = AgentStatus.Running;
      if (string.IsNullOrWhiteSpace(UserPrompt)) {
        Status = AgentStatus.Error;
        _logger.LogError("ClaudeCodeBaseAgent UserPrompt cannot be null or empty.");
        throw new InvalidOperationException("UserPrompt cannot be null or empty.");
      }
      Model = _factorySettingsService.ClaudeModel;

      var log = new AgentLog {
        AgentName = this.Name,
        SystemPrompt = this.SystemPrompt,
        UserPrompt = this.UserPrompt,
        Established = DateTime.UtcNow
      };

      try {
        // Write MCP config for this invocation based on ToolsToUse
        // need to make sure the .mcp.json file has accurate location pointing to
        // StorytimeMCP published location. no args the rest of the file is created when app runs.

        // Build claude -p arguments
        var args = $"-p \"{EscapeArg(UserPrompt)}\"";

        if (!string.IsNullOrWhiteSpace(SystemPrompt))
          args += $" --system-prompt \"{EscapeArg(SystemPrompt)}\"";

        args += $" --model {Model}";
        args += " --dangerously-skip-permissions --output-format text";

        var psi = new ProcessStartInfo {
          FileName = "claude", // e.g. "claude" or full path
          Arguments = args,
          WorkingDirectory = Cx.ClaudeExecutablePath,
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          UseShellExecute = false,
          CreateNoWindow = false,
          StandardOutputEncoding = System.Text.Encoding.UTF8,
          StandardErrorEncoding = System.Text.Encoding.UTF8
        };

        using var process = new Process { StartInfo = psi };
        process.Start();

        var stdout = await process.StandardOutput.ReadToEndAsync(cancellationToken);
        var stderr = await process.StandardError.ReadToEndAsync(cancellationToken);
        await process.WaitForExitAsync(cancellationToken);

        if (process.ExitCode != 0)
          throw new Exception($"claude -p exited with code {process.ExitCode}: {stderr}");

        // Wrap stdout in a ChatResponse shape so the rest of the pipeline is identical
        var response = new ChatResponse {
           Output = new List<OutputItem> {
             new MessageOutputItem {
                Content = stdout.Trim()               
             }
           }          
        };

        log.ToolCallsSummary = "";
        log.RawResponse = stdout;
        log.Success = true;
        LastResponse = response;
        Status = AgentStatus.Idle;
        return response;

      } catch (Exception ex) {
        Status = AgentStatus.Error;
        log.Success = false;
        log.ErrorMessage = ex.Message;
        _logger.LogError(ex, "Error invoking ClaudeCodeBaseAgent.");
        throw;
      } finally {
        await _context.AgentLogs.AddAsync(log);
        await _context.SaveChangesAsync();
      }
    }
    private static string EscapeArg(string s) =>
        s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", " ").Replace("\r", "");
    }

  public class ClaudeCodeSettings {
    public string ClaudeExecutablePath { get; set; } = "claude";
    public Dictionary<string, object> McpServers { get; set; } = new();
  }
}
