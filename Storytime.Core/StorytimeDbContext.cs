using KB.Core;
using KB.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Storytime.Core.Entities;


namespace Storytime.Core {
  public class StorytimeDbContext : KbDbContext {
    public StorytimeDbContext(DbContextOptions<StorytimeDbContext> options) : base(options) { }
    public DbSet<AgentLog> AgentLogs => Set<AgentLog>();
    public DbSet<AgentQueueItem> AgentQueue { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);      
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(StorytimeDbContext).Assembly);

      // ====================== STORYTIME SEEDS ======================
      modelBuilder.Entity<ItemType>().HasData(
          new ItemType { Id = 1, Name = "Project", Description = "Top-level story container (e.g. The Lost Drifter)" },
          new ItemType { Id = 2, Name = "Story", Description = "A full narrative arc inside a Project" },
          new ItemType { Id = 3, Name = "Scene", Description = "Self-contained chunk with a clear goal" },
          new ItemType { Id = 4, Name = "Beat", Description = "Single moment inside a Scene (Setup/Choice/Escalation/Climax/ResolutionHook)" },
          new ItemType { Id = 5, Name = "Character", Description = "NPC or main traveler" },
          new ItemType { Id = 6, Name = "Location", Description = "Scene location" },
          new ItemType { Id = 7, Name = "Rule", Description = "Project bible rule" },
          new ItemType { Id = 8, Name = "Tone", Description = "Project bible tone/mood reference" },
          new ItemType { Id = 9, Name = "CallSheet", Description = "Director's ordered cast sequence for a Scene" },
          new ItemType { Id = 10, Name = "Performance", Description = "Executed result of a CallSheet by character agents" },
          new ItemType { Id = 11, Name = "Deliverable", Description = "Final rendered output — prose, podcast, video, etc." },
          new ItemType { Id = 12, Name = "Narration", Description = "Narration item for call sheets" }
      );

      modelBuilder.Entity<ItemRelationType>().HasData(
          new ItemRelationType { Id = 1, Relation = "Contains", Description = "Parent → child (Project contains Story, Story contains Scene, etc.)" },
          new ItemRelationType { Id = 4, Relation = "UsesRule", Description = "Any item → global rule from the project bible" },
          new ItemRelationType { Id = 5, Relation = "FeaturesCharacter", Description = "Scene/Beat features a character" },
          new ItemRelationType { Id = 6, Relation = "TakesPlaceAt", Description = "Scene/Beat location reference" },
          new ItemRelationType { Id = 7, Relation = "UsesTone", Description = "Any item → tone reference" },
          new ItemRelationType { Id = 8, Relation = "DirectedAs", Description = "Scene → CallSheet (Director's interpretation of curated beats)" },
          new ItemRelationType { Id = 9, Relation = "Produces", Description = "CallSheet → Performance, or Performance → Deliverable" },
          new ItemRelationType { Id = 10, Relation = "HasRole", Description = "CallSheet → Character (Rank on the relation defines cast order)" },
          new ItemRelationType { Id = 11, Relation = "Narrates", Description = "CallSheet → Narration (Defines the narration for a scene, with rank defining order if multiple)" }
      );
      
    }
  }

  public enum StItemType {
    Project = 1,
    Story = 2,
    Scene = 3,
    Beat = 4,
    Character = 5,
    Location = 6,
    Rule = 7,
    Tone = 8,
    CallSheet = 9,
    Performance = 10,
    Deliverable = 11,
    Narration = 12
  }
  public enum StRelationType {
    Contains = 1,    
    UsesRule = 4,
    FeaturesCharacter = 5,
    TakesPlaceAt = 6,
    UsesTone = 7,
    DirectedAs = 8,
    Produces = 9,
    HasRole = 10,
    Narrates = 11
  }
}
