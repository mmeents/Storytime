using KB.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Storytime.Core.Agents;
using Storytime.Core.Clients;
using Storytime.Core.Service;
using Storytime.Core.Tools;

namespace Storytime.Core {
  public static class DependencyInjection {
    public static IServiceCollection AddStorytimeCore<TContext>(this IServiceCollection services, IConfiguration configuration) where TContext : KbDbContext {
      services.AddDbContext<TContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

      services.AddMediatR(cfg => {
        cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);  // Storytime.Core
        //cfg.RegisterServicesFromAssembly(typeof(KbDbContext).Assembly);           // KB.Core
      });
      services.AddSingleton<IFactorySettingsService, FactorySettingsService>();
      services.AddScoped<IAppDataModuleService, AppDataModuleService>();    
      services.AddScoped<ILmStudioClient, LmStudioClient>();
      services.AddScoped<ILocalBaseAgentFactory, LocalBaseAgentFactory>();
      services.AddScoped<LocalBaseAgent>();
      services.AddScoped<ClaudeCodeBaseAgent>();
      services.AddScoped<IDevelopmentManagerAgent, DevelopmentManagerAgent>();
      services.AddScoped<IStoryWriterAgent, StoryWriterAgent>();
      services.AddScoped<ISceneWriterAgent, SceneWriterAgent>();
      services.AddScoped<IDirectorAgent, DirectorAgent>();
      services.AddScoped<ISetAgent, SetAgent>();
      services.AddScoped<IObserverAgent, ObserverAgent>();

      services.AddScoped<IAppSettingService, AppSettingService>();
      return services;
    }

    public static IServiceCollection AddStorytimeMcpCore(this IServiceCollection services, IConfiguration configuration) {
       services.AddStorytimeCore<StorytimeDbContext>(configuration);
       services.AddScoped<IStorytimeToolsHandler, StorytimeToolsHandler>();
       services.AddScoped<IStDevToolsHandler, StDevToolsHandler>();
       services.AddScoped<IStProductionToolsHandler, StProductionToolsHandler>();
       services.AddHostedService<StorytimeMcpHostedService>();
      return services;
    }
  

  }
}
