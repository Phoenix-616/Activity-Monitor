using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models
{
    public class DeveloperRepository
    {
        public Guid DeveloperId { get; set; }
        public Developer Developer { get; set; }
        public Guid RepositoryId { get; set; }
        public Repository Repository { get; set; }
    }

    class DeveloperRepositoryConfiguration : IEntityTypeConfiguration<DeveloperRepository>
    {
        public void Configure(EntityTypeBuilder<DeveloperRepository> builder)
        {
            builder
                .HasKey(x => new { x.DeveloperId, x.RepositoryId });

            builder
                .HasOne(x => x.Developer)
                .WithMany(x => x.Repositories)
                .HasForeignKey(x => x.DeveloperId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Repository)
                .WithMany(x => x.Developers)
                .HasForeignKey(x => x.RepositoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
