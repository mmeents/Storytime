using Microsoft.EntityFrameworkCore;
using Storytime.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Storytime.Core.Service {

  public interface IAppSettingService { }
  public class AppSettingService : IAppSettingService {
    private readonly StorytimeDbContext _context;
    public AppSettingService(StorytimeDbContext context) {
      _context = context;
    }

    public AppSetting? this[string key] {
      get {
        var setting = _context.AppSettings.FirstOrDefault(s => s.Key == key);
        return setting;
      }
      set {
        var existingSetting = _context.AppSettings.FirstOrDefault(s => s.Key == key);
        if (existingSetting != null) {
          if (value == null) {
            _context.AppSettings.Remove(existingSetting);
            _context.SaveChanges();
            return;
          }
          existingSetting.UpdateValue(value.Value, value.ValueInt);
          _context.AppSettings.Update(existingSetting);
          _context.SaveChanges();
        } else {
          if (value == null) {
            return;
          }
          var newSetting = new AppSetting(key, value.Value, value.ValueInt);
          _context.AppSettings.Add(newSetting);
          _context.SaveChanges(); 
        }

      }
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default) {
      return await _context.AppSettings.AnyAsync(s => s.Key == key, cancellationToken);
    }

    public async Task<AppSetting?> GetByIdAsync(int id, CancellationToken cancellationToken = default) {
      return await _context.AppSettings
          .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<AppSetting?> GetByKeyAsync(string key, CancellationToken cancellationToken = default) {
      return await _context.AppSettings
          .FirstOrDefaultAsync(s => s.Key == key, cancellationToken);
    }

    public async Task<List<AppSetting>> GetAllAsync(CancellationToken cancellationToken = default) {
      return await _context.AppSettings
          .OrderBy(s => s.Key)
          .ToListAsync(cancellationToken);
    }

    public async Task<Dictionary<string, string?>> GetAllAsDictionaryAsync(CancellationToken cancellationToken = default) {
      return await _context.AppSettings
          .ToDictionaryAsync(s => s.Key, s => s.Value, cancellationToken);
    }





  }
}
