using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ActivityMonitor.Database.Models
{
    public class DeveloperRepository
    {
        public int DeveloperId { get; set; }
        public Developer Developer { get; set; }
        public int RepositoryId { get; set; }
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