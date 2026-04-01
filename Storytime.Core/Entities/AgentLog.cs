using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storytime.Core.Entities {
  public class AgentLog {    
    public int Id { get; set; } = 0;
    public string AgentName { get; set; } = "";
    public int ContextItemId { get; set; } = 0;
    public string SystemPrompt { get; set; } = "";
    public string UserPrompt { get; set; } = "";    
    public string ToolCallsSummary { get; set; } = "";
    public bool Success { get; set; } = true;
    public string? ErrorMessage { get; set; } = null;
    public string RawResponse { get; set; } = "";
    public DateTime Established { get; set; } = DateTime.Now;

  }

  public class AgentLogConfiguration : IEntityTypeConfiguration<AgentLog> {
    public void Configure(EntityTypeBuilder<AgentLog> builder) {
      builder.ToTable("AgentLogs");
      builder.HasKey(al => al.Id);
      builder.Property(al => al.Id).ValueGeneratedOnAdd();
      builder.Property(al => al.AgentName).IsRequired().HasMaxLength(500);
      builder.Property(al => al.ContextItemId).IsRequired();
      builder.Property(al => al.SystemPrompt).IsRequired().HasMaxLength(-1);
      builder.Property(al => al.UserPrompt).IsRequired().HasMaxLength(-1);      
      builder.Property(al => al.ToolCallsSummary).IsRequired().HasMaxLength(-1);
      builder.Property(al => al.Success).IsRequired().HasDefaultValue(true);
      builder.Property(al => al.ErrorMessage).IsRequired(false).HasMaxLength(-1);
      builder.Property(al => al.RawResponse).IsRequired().HasMaxLength(-1);
      builder.Property(al => al.Established).IsRequired().HasDefaultValueSql("GETUTCDATE()");

      builder.HasIndex(al => al.AgentName);     
      builder.HasIndex(al => al.ContextItemId); 
      builder.HasIndex(al => al.Established);   
      builder.HasIndex(al => al.Success);       
    }
  }


}
