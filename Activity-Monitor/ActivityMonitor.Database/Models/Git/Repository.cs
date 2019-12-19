using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace ActivityMonitor.Database.Models
{
    public class Repository
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<DeveloperRepository> Developers { get; set; }
        public ICollection<Commit> Commits { get; set; }
        public ICollection<File> Files { get; set; }
        public ICollection<CodeComplexity> CodeComplexityMeasures { get; set; }
    }

    class RepositoryConfig : IEntityTypeConfiguration<Repository>
    {
        public void Configure(EntityTypeBuilder<Repository> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired();

            builder
                .HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}