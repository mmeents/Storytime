using Storytime.Core.Clients;
using Storytime.Core.Constants;
using Storytime.Core.Entities;
using Storytime.Core.Models;
using Storytime.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Storytime.Core.Service.IFactorySettingsService;

namespace Storytime.Core.Agents {
  public interface ILocalBaseAgent {
    double Temperature { get; set; }
    string Model { get; set; }
    string Name { get; set; }
    string SystemPrompt { get; set; }
    string UserPrompt { get; set; }
    List<string> ToolsToUse { get; set; }
    ChatRequest? LastRequest { get; set; }
    ChatResponse? LastResponse { get; set; }
    List<ChatRequest> ChatRequests { get; set; }
    List<ChatResponse> ChatResponses { get; set; }

    AgentStatus Status { get; }

    
    Task<ChatResponse> InvokeAgentAsync(CancellationToken cancellationToken);

  }

  public enum AgentStatus { Idle, Running, Error }

  public class LocalBaseAgent : ILocalBaseAgent {
    private readonly ILmStudioClient _lmStudioClient;
    private readonly StorytimeDbContext _context;
    private IFactorySettingsService _factorySettingsService;
    public LocalBaseAgent(ILmStudioClient lmStudioClient, StorytimeDbContext context, IFactorySettingsService factorySettingsService) {
      _lmStudioClient = lmStudioClient;
      _context = context;
      _factorySettingsService = factorySettingsService;
      Model = _factorySettingsService.LMStudioModel;
    }
    
    public AgentStatus Status { get; private set; } = AgentStatus.Idle;
    public double Temperature { get; set; } = 0.8;
    public string Model { get; set; }

    public string Name { get; set; } = "";   
    public string SystemPrompt { get; set; } = "";
    public string UserPrompt { get; set; } = "";
    public List<string> ToolsToUse { get; set; } = new List<string>();

    private ChatRequest? _lastRequest;
    public ChatRequest? LastRequest {
      get => _lastRequest;
      set {
        _lastRequest = value;
        if (value != null) {
          ChatRequests.Add(value);
        }
      }
    }

    private ChatResponse? _lastResponse;
    public ChatResponse? LastResponse {
      get => _lastResponse;
      set {
        _lastResponse = value;
        if (value != null) {
          ChatResponses.Add(value);
        }
      }
    }

    public List<ChatRequest> ChatRequests { get; set; } = new List<ChatRequest>();
    public List<ChatResponse> ChatResponses { get; set; } = new List<ChatResponse>();
    
    public async Task<ChatResponse> InvokeAgentAsync(CancellationToken cancellationToken) {
      if (Status == AgentStatus.Running) {
        throw new InvalidOperationException("Agent is already running.");
      }
      Status = AgentStatus.Running;
      if (string.IsNullOrWhiteSpace(UserPrompt)) {
        Status = AgentStatus.Error;
        throw new InvalidOperationException("UserPrompt cannot be null or empty.");
      }
      Model = _factorySettingsService.LMStudioModel;
      var log = new AgentLog {
        AgentName = this.Name,
        SystemPrompt = this.SystemPrompt,
        UserPrompt = this.UserPrompt,
        Established = DateTime.UtcNow
      };
      try { 

        List<Integration> listIntegrations = ToolsToUse
          .Select(toolName => (Integration) new PluginIntegration { Id = toolName }).ToList();

        var request = new ChatRequest {
          Model = this.Model,
          Input = this.UserPrompt,
          SystemPrompt = this.SystemPrompt,
          Temperature = this.Temperature,
          ContextLength = Cx.DefaultLmStudioContextLength,
          Integrations = listIntegrations
        };

        LastRequest = request;
    
        var response = await _lmStudioClient.ChatAsync(request, cancellationToken);
        response.GetText();
        
        log.ToolCallsSummary = response.GetText(); 
        log.RawResponse = JsonSerializer.Serialize(response);
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

  }
}
