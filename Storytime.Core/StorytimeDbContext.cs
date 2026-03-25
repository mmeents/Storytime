using KB.Core;
using KB.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace Storytime.Core {
  public class StorytimeDbContext : KbDbContext {
    public StorytimeDbContext(DbContextOptions<StorytimeDbContext> options) : base(options) { }

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
          new ItemType { Id = 7, Name = "Rule", Description = "Project bible rule" }
      
      );

      modelBuilder.Entity<ItemRelationType>().HasData(
          new ItemRelationType { Id = 1, Relation = "Contains", Description = "Parent → child (Project contains Story, Story contains Scene, etc.)" },
          new ItemRelationType { Id = 2, Relation = "HasBeat", Description = "Scene → ordered Beat (with order stored in the Beat's Data JSON)" },
          new ItemRelationType { Id = 3, Relation = "NextBeat", Description = "Beat → next Beat (optional explicit override)" },
          new ItemRelationType { Id = 4, Relation = "UsesRule", Description = "Any item → global rule from the project bible" },
          new ItemRelationType { Id = 5, Relation = "FeaturesCharacter", Description = "Scene/Beat features a character" },
          new ItemRelationType { Id = 6, Relation = "TakesPlaceAt", Description = "Scene/Beat location reference" },
          new ItemRelationType { Id = 7, Relation = "UsesTone", Description = "Any item → tone reference" }
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
    Rule = 7
  }
   public enum StRelationType {
    Contains = 1,
    HasBeat = 2,
    NextBeat = 3,
    UsesRule = 4,
    FeaturesCharacter = 5,
    TakesPlaceAt = 6
  }
}
