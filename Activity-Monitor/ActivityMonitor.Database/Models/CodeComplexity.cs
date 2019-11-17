using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models
{
    public class CodeComplexity
    {
        public Guid Id { get; set; }
        public Guid RepositoryId { get; set; }
        public Repository Repository { get; set; }
        public DateTimeOffset CalculatedAt { get; set; }
        public int Value { get; set; }
    }

    class CodeComplexityConfiguration : IEntityTypeConfiguration<CodeComplexity>
    {
        public void Configure(EntityTypeBuilder<CodeComplexity> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasIndex(x => x.CalculatedAt);

            builder
                .HasIndex(x => x.RepositoryId);
            builder
                .HasOne(x => x.Repository)
                .WithMany(x => x.CodeComplexityMeasures)
                .HasForeignKey(x => x.RepositoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
