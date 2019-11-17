using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models
{
    public enum FileAction
    {
        Created = 0,
        Modified = 1,
        Deleted = 2
    }

    public class CommitFile
    {
        public Guid CommitId { get; set; }
        public Commit Commit { get; set; }
        public Guid FileId { get; set; }
        public File File { get; set; }
        public FileAction Action { get; set; }

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
