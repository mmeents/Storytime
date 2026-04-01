using Storytime.Core.Entities;
using Storytime.Core.Models;
using Storytime.Core.Constants;
using System.Diagnostics;
using System.Text;

namespace Storytime.Core.Agents {
  /// <summary>
  /// Sorry this is untested... 
  /// Drives GitHub Copilot CLI as a local agent in non-interactive mode.
  /// Invocation: gh copilot -p "{prompt}" --allow-all-tools --silent
  ///   -p / --prompt     : non-interactive mode, prompt passed directly
  ///   --allow-all-tools : permits MCP tool calls (no URL/path access)
  ///   --silent          : output response only, no interactive chrome
  /// LMStudioModel is selected via Copilot's own routing — no --model flag available yet.
  /// MCP config is picked up from Copilot's configured MCP servers.
  /// </summary>
  public class CopilotCliBaseAgent : ILocalBaseAgent {
    private readonly StorytimeDbContext _context;
    public CopilotCliBaseAgent(StorytimeDbContext context) {
      _context = context;
    }

    public AgentStatus Status { get; private set; } = AgentStatus.Idle;
    public double Temperature { get; set; } = 0.8;
    public string Model { get; set; } = ""; // Copilot routes model internally — no CLI flag available yet
    public string Name { get; set; } = "";
    public string SystemPrompt { get; set; } = "";
    public string UserPrompt { get; set; } = "";
    public List<string> ToolsToUse { get; set; } = new();
    public ChatRequest? LastRequest { get; set; }
    public ChatResponse? LastResponse { get; set; }
    public List<ChatRequest> ChatRequests { get; set; } = new();
    public List<ChatResponse> ChatResponses { get; set; } = new();

    public async Task<ChatResponse> InvokeAgentAsync(CancellationToken cancellationToken) {
      if (Status == AgentStatus.Running)
        throw new InvalidOperationException("Agent is already running.");

      Status = AgentStatus.Running;
      if (string.IsNullOrWhiteSpace(UserPrompt)) {
        Status = AgentStatus.Error;
        throw new InvalidOperationException("UserPrompt cannot be null or empty.");
      }

      var log = new AgentLog {
        AgentName = this.Name,
        SystemPrompt = this.SystemPrompt,
        UserPrompt = this.UserPrompt,
        Established = DateTime.UtcNow
      };

      try {
        // Combine system prompt into user prompt if provided
        // gh copilot -p does not expose a --system-prompt flag
        var fullPrompt = string.IsNullOrWhiteSpace(SystemPrompt)
          ? UserPrompt
          : $"{SystemPrompt}\n\n{UserPrompt}";

        var args = $"copilot -p \"{EscapeArg(fullPrompt)}\" --allow-all-tools --silent";

        var psi = new ProcessStartInfo {
          FileName = "gh",
          Arguments = args,
          WorkingDirectory = Cx.ClaudeExecutablePath, // gh should be on PATH; adjust Cx if needed
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          UseShellExecute = false,
          CreateNoWindow = false,
          StandardOutputEncoding = Encoding.UTF8,
          StandardErrorEncoding = Encoding.UTF8
        };

        using var process = new Process { StartInfo = psi };
        process.Start();

        var stdout = await process.StandardOutput.ReadToEndAsync(cancellationToken);
        var stderr = await process.StandardError.ReadToEndAsync(cancellationToken);
        await process.WaitForExitAsync(cancellationToken);

        if (process.ExitCode != 0)
          throw new Exception($"gh copilot exited with code {process.ExitCode}: {stderr}");

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
        throw;
      } finally {
        await _context.AgentLogs.AddAsync(log);
        await _context.SaveChangesAsync();
      }
    }

    private static string EscapeArg(string s) =>
        s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", " ").Replace("\r", "");
  }
}
