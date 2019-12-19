using ActivityMonitor.Database.Models.Git.ActivityMonitor.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace ActivityMonitor.Database.Models
{
    public class Commit
    {
        public int Id { get; set; }
        /// <summary>
        /// Id in git-system
        /// </summary>
        public string GitId { get; set; }
        public int AuthorId { get; set; }
        public Developer Author { get; set; }
        public int RepositoryId { get; set; }
        public Repository Repository { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public ICollection<CommitFile> Files { get; set; }
        public ICollection<CodeString> CreatedStrings { get; set; }
        public ICollection<CodeString> DeletedStrings { get; set; }
        public int Created { get; set; }
        public int Deleted { get; set; }
    }

    class CommitConfiguration : IEntityTypeConfiguration<Commit>
    {
        public void Configure(EntityTypeBuilder<Commit> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.GitId)
                .IsRequired();
            builder
                .HasIndex(x => x.GitId)
                .IsUnique();

            builder
                .HasIndex(x => x.CreatedAt);

            builder
                .HasIndex(x => x.AuthorId);
            builder
                .HasOne(x => x.Author)
                .WithMany(x => x.Commits)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasIndex(x => x.RepositoryId);
            builder
                .HasOne(x => x.Repository)
                .WithMany(x => x.Commits)
                .HasForeignKey(x => x.RepositoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}