using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storytime.Core.Entities {
  public class AgentQueueItem {
    public int Id { get; set; }
    public int ItemId { get; set; }
    public StItemType TargetDepth { get; set; }
    public AgentQueueStatus Status { get; set; } = AgentQueueStatus.Pending;
    public DateTime ScheduledAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }
  }

  public enum AgentQueueStatus { Pending, Running, Completed, Failed, Cancelled }

  public class AgentQueueItemConfiguration : IEntityTypeConfiguration<AgentQueueItem> {
    public void Configure(EntityTypeBuilder<AgentQueueItem> builder) {
      builder.ToTable("AgentQueue");

      builder.HasKey(x => x.Id);
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.ItemId).IsRequired();
      builder.Property(x => x.TargetDepth).IsRequired().HasConversion<int>();
      builder.Property(x => x.Status).IsRequired().HasConversion<int>().HasDefaultValue(AgentQueueStatus.Pending);
      builder.Property(x => x.ScheduledAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
      builder.Property(x => x.StartedAt).IsRequired(false);
      builder.Property(x => x.CompletedAt).IsRequired(false);
      builder.Property(x => x.ErrorMessage).IsRequired(false).HasMaxLength(2000);

      builder.HasIndex(x => x.Status);
      builder.HasIndex(x => x.ScheduledAt);
    }
  }


}
