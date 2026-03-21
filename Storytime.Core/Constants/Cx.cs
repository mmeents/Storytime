using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storytime.Core.Constants {
  public static class Cx {
    public static string AppName => "Storytime";
    public static string AppVersion => "1.0.0";
    public const string ApiLocalPort = "44344";
    public const string ApiLocalhostUrl = $"https://localhost:{ApiLocalPort}";  // via iis express 

    public static string CommonAppPath {
      get {
        string commonPath = Path.Combine(
          Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
          Cx.AppName).ResolvePath();
        if (!Directory.Exists(commonPath)) {
          Directory.CreateDirectory(commonPath);
        }
        return commonPath;
      }
    }

    public static string LogsAppPath {
      get {
        string logsPath = Path.Combine(CommonAppPath, "logs").ResolvePath();
        if (!Directory.Exists(logsPath)) {
          Directory.CreateDirectory(logsPath);
        }
        return logsPath;
      }
    }


    public static string ResolvePath(this string path) {
      if (!Path.IsPathRooted(path)) {
        return Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), path));
      }
      return Path.GetFullPath(path);
    }

  }
}
