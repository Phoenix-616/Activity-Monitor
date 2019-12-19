using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models
{
    public class Journal
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedOn{ get; set; }
        public string NameChange { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public int IssueId { get; set; }
    }

    class JournalConfiguration : IEntityTypeConfiguration<Journal>
    {
        public void Configure(EntityTypeBuilder<Journal> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Notes)
                .IsRequired();

            builder
                .Property(x => x.CreatedOn)
                .IsRequired();

            builder
                .Property(x => x.NameChange)
                .IsRequired();

            builder
                .Property(x => x.OldValue)
                .IsRequired();

            builder
                .Property(x => x.NewValue)
                .IsRequired();

            builder
                .HasIndex(x => x.IssueId);

            builder
                .HasIndex(x => x.AuthorId);
        }
    }
}
