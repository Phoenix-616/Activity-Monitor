using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models
{
    class Issue
    {
        public int Id { get; set; }
        public string TrackerName { get; set; }
        public int AuthorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }

        public int ProjectId { get; set; }
    }

    class IssueConfiguration : IEntityTypeConfiguration<Issue>
    {
        public void Configure(EntityTypeBuilder<Issue> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.TrackerName)
                .IsRequired();

            builder
                .HasIndex(x => x.ProjectId);

            builder
                .HasIndex(x => x.AuthorId);
        }
    }
}
