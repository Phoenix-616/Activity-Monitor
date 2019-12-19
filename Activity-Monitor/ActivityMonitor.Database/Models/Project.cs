using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models
{
    class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime Created_on { get; set; }
        public DateTime Updated_on { get; set; }
        public ICollection<ProjectMembership> Memberships { get; set; }
        public ICollection<Issue> Issues { get; set; }

    }

    class ProjectConfig : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired();

            builder
                .Property(x => x.Description)
                .IsRequired();

            builder
                .Property(x => x.Status)
                .IsRequired();

            builder
                .Property(x => x.Created_on)
                .IsRequired();

            builder
                .Property(x => x.Updated_on)
                .IsRequired();
        }
    }
}
