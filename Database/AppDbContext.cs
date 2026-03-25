using Microsoft.EntityFrameworkCore;
using PromptVault.Api.Models;

namespace PromptVault.Api.Database
{
    public class AppDbContext : DbContext
    {
        // Takes in settings like the connection string from program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        //Models
        public DbSet<Prompt> Prompts { get; set; }
        public DbSet<PromptTag> PromptTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TestResult> Results { get; set; }


        // Configure special rules for the database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tells PromptTag that it has a primary key that i combined by PromptId and TagId
            modelBuilder.Entity<PromptTag>()
                .HasKey(pt => new { pt.PromptId, pt.TagId });
        }
    }
}
