using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models
{
    public class CodeString
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public File File { get; set; }
        public int CreationCommitId { get; set; }
        public Commit CreationCommit { get; set; }
        public int? DeleteCommitId { get; set; }
        public Commit DeleteCommit { get; set; }
    }

    class CodeStringConfig : IEntityTypeConfiguration<CodeString>
    {
        public void Configure(EntityTypeBuilder<CodeString> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasOne(x => x.File)
                .WithMany(x => x.Strings)
                .HasForeignKey(x => x.FileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.CreationCommit)
                .WithMany(x => x.CreatedStrings)
                .HasForeignKey(x => x.CreationCommitId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.DeleteCommit)
                .WithMany(x => x.DeletedStrings)
                .HasForeignKey(x => x.DeleteCommitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}