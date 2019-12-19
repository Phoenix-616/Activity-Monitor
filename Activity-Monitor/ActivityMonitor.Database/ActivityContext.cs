using ActivityMonitor.Database.Models;
using ActivityMonitor.Database.Models.Git.ActivityMonitor.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityMonitor.Database
{
    public class ActivityContext : DbContext
    {
        //public ActivityContext(DbContextOptions<ActivityContext> options)
        //   : base(options)
        //{
        //}

        //Git
        public DbSet<CodeComplexity> CodeComplexitySamples { get; set; }
        public DbSet<CodeString> CodeStrings { get; set; }
        public DbSet<Commit> Commits { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<CommitFile> CommitFiles { get; set; }
        public DbSet<DeveloperRepository> DeveloperRepositories { get; set; }
        // Redmine
        public DbSet<Project> Projects { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<ProjectMembership> ProjectMemberships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ActivityContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=123456789");
        }
    }
}
