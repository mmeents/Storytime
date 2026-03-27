using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MCPSharp;
using Storytime.Core.Constants;
using Storytime.Core.Tools;



namespace Storytime.Core.Service {
  public class StorytimeMcpHostedService : BackgroundService {
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<StorytimeMcpHostedService> _logger;
    public StorytimeMcpHostedService(IServiceProvider serviceProvider, ILogger<StorytimeMcpHostedService> logger) {
      _serviceProvider = serviceProvider;
      _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
      await Task.Delay(3000);
      _logger.LogInformation("🚀 Storytime MCP Server starting");
      DiBridgeService.Initialize(_serviceProvider);
      MCPServer.Register<StorytimeTools>();
      await MCPServer.StartAsync(Cx.McpAppName, Cx.AppVersion);
    }
  }
}
