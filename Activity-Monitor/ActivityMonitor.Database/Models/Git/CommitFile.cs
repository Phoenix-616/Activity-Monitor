namespace ActivityMonitor.Database.Models.Git
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    namespace ActivityMonitor.Database.Models
    {
        public class CommitFile
        {
            public int CommitId { get; set; }
            public Commit Commit { get; set; }
            public int FileId { get; set; }
            public File File { get; set; }

        }

        class CommitFileConfig : IEntityTypeConfiguration<CommitFile>
        {
            public void Configure(EntityTypeBuilder<CommitFile> builder)
            {
                builder
                    .HasKey(x => new { x.CommitId, x.FileId });

                builder
                    .HasOne(x => x.Commit)
                    .WithMany(x => x.Files)
                    .HasForeignKey(x => x.CommitId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder
                    .HasOne(x => x.File)
                    .WithMany(x => x.Commits)
                    .HasForeignKey(x => x.FileId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }
    }
}
