using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models
{
    public class File
    {
        public Guid Id { get; set; }
        public Guid RepositoryId { get; set; }
        public Repository Repository { get; set; }
        public string Name { get; set; }
        public ICollection<CommitFile> Commits { get; set; }
        public ICollection<CodeString> Strings { get; set; }
    }

    class FileConfig : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasIndex(x => x.RepositoryId);
            builder
                .HasOne(x => x.Repository)
                .WithMany(x => x.Files)
                .HasForeignKey(x => x.RepositoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(x => x.Name)
                .IsRequired();
        }
    }
}
