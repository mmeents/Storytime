using StorytimeAr.Extensions;
using Microsoft.Extensions.DependencyInjection;
namespace StorytimeAr {
  internal static class Program {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {      
      ApplicationConfiguration.Initialize();
      using var host = AppServiceExts.BuildHost();
      var form1 = host.Services.GetRequiredService<Form1>();
      Application.Run(form1);
    }
  }
}