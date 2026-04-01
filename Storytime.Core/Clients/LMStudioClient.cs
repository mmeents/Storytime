using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Storytime.Core.Models;
using Storytime.Core.Constants;

namespace Storytime.Core.Clients {
  public interface ILmStudioClient {
    public string BaseUrl { get; set; }
    Task<ModelsResponse> GetModelsAsync(CancellationToken ct = default);
    Task<List<LmModel>> GetLlmModelsAsync(CancellationToken ct = default);
    Task<ChatResponse> ChatAsync(ChatRequest request, CancellationToken ct = default);
    Task<ChatResponse> ChatAsync(
        string model,
        string prompt,
        string? systemPrompt = null,
        double temperature = 0,
        int? contextLength = null,
        CancellationToken ct = default);
  }

  public class LmStudioClient : ILmStudioClient, IDisposable {

    private static readonly JsonSerializerOptions JsonOptions = new() {
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
      PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };
    private readonly HttpClient _http;
    private string _baseUrl = Cx.LMStudioUrl;
    private string? _apiToken = Cx.LMStudioApiKey;
    public string BaseUrl {
      get => _baseUrl;
      set {
        _baseUrl = value;
        if (_http != null) {
          _http.BaseAddress = new Uri(_baseUrl.TrimEnd('/') + "/");          
        }
      }
    }
    
    public string? ApiToken {
      get => _apiToken; set {
        _apiToken = value;
        if (_http != null) {
          if (!string.IsNullOrWhiteSpace(_apiToken))
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiToken);
        }
      }
    }
        
    public LmStudioClient() {
      _http = new HttpClient { BaseAddress = new Uri(BaseUrl.TrimEnd('/') + "/"), Timeout = Timeout.InfiniteTimeSpan };
      BaseUrl = Cx.LMStudioUrl;
      ApiToken = Cx.LMStudioApiKey;
    }
    public LmStudioClient(string baseUrl, string? apiToken = null) {
      _http = new HttpClient { BaseAddress = new Uri(BaseUrl.TrimEnd('/') + "/"), Timeout = Timeout.InfiniteTimeSpan };
      BaseUrl = baseUrl;
      ApiToken = apiToken;
    }

    // ─────────────────────────────────────────────────────────
    // MODELS
    // ─────────────────────────────────────────────────────────

    /// <summary>GET /api/v1/models</summary>
    public async Task<ModelsResponse> GetModelsAsync(CancellationToken ct = default) {
      var response = await _http.GetAsync("api/v1/models", ct);
      response.EnsureSuccessStatusCode();
      return await response.Content.ReadFromJsonAsync<ModelsResponse>(JsonOptions, ct)
             ?? throw new InvalidOperationException("Null response from /api/v1/models");
    }

    /// <summary>Convenience: returns only LLM-type models.</summary>
    public async Task<List<LmModel>> GetLlmModelsAsync(CancellationToken ct = default) {
      var result = await GetModelsAsync(ct);
      return result.Models.Where(m => m.Type == "llm").ToList();
    }

    // ─────────────────────────────────────────────────────────
    // CHAT
    // ─────────────────────────────────────────────────────────

    /// <summary>POST /api/v1/chat</summary>
    public async Task<ChatResponse> ChatAsync(ChatRequest request, CancellationToken ct = default) {
      var httpResponse = await _http.PostAsJsonAsync("api/v1/chat", request, JsonOptions, ct);
      httpResponse.EnsureSuccessStatusCode();
      return await httpResponse.Content.ReadFromJsonAsync<ChatResponse>(JsonOptions, ct)
             ?? throw new InvalidOperationException("Null response from /api/v1/chat");
    }

    /// <summary>Quick single-prompt helper.</summary>
    public Task<ChatResponse> ChatAsync(
        string model,
        string prompt,
        string? systemPrompt = null,
        double temperature = 0,
        int? contextLength = null,
        CancellationToken ct = default) {

      var request = new ChatRequest {
        Model = model,
        Input = prompt,
        SystemPrompt = systemPrompt,
        Temperature = temperature,
        ContextLength = contextLength,
        Integrations = [new PluginIntegration { Id = Cx.LMStudioStorytimeMcpToolName }]
      };
      var req = ChatRequest.Simple(model, prompt, systemPrompt);
      req.Temperature = temperature;
      req.ContextLength = contextLength;
      return ChatAsync(req, ct);
    }

    public void Dispose() => _http.Dispose();
  }
}
