using ActivityMonitor.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database
{
    public class ActivityContext : DbContext
    {
        public ActivityContext(DbContextOptions<ActivityContext> options)
           : base(options)
        {
        }

        public DbSet<CodeComplexity> CodeComplexitySamples { get; set; }
        public DbSet<CodeString> CodeStrings { get; set; }
        public DbSet<Commit> Commits { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Repository> Repositories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ActivityContext).Assembly);
        }
    }
}
