using KB.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Storytime.Core {
  public static class DependencyInjection {
    public static IServiceCollection AddStorytimeCore<TContext>(this IServiceCollection services, IConfiguration configuration) where TContext : KbDbContext {
      services.AddDbContext<TContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

      services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));


      return services;
    }

  }
}
