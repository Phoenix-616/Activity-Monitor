using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models
{
    class ProjectMembership
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int MembershipId { get; set; }
        public Membership Membership { get; set; }
    }

    class ProjectMembershipConfig : IEntityTypeConfiguration<ProjectMembership>
    {
        public void Configure(EntityTypeBuilder<ProjectMembership> builder)
        {
            builder
                .HasKey(x => new { x.ProjectId, x.MembershipId });

            builder
                .HasOne(x => x.Project)
                .WithMany(x => x.Memberships)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Membership)
                .WithMany(x => x.Projects)
                .HasForeignKey(x => x.MembershipId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
