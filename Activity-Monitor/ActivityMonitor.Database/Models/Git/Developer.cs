using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace ActivityMonitor.Database.Models
{
    public class Developer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public ICollection<DeveloperRepository> Repositories { get; set; }
        public ICollection<Commit> Commits { get; set; }

        class DeveloperConfig : IEntityTypeConfiguration<Developer>
        {
            public void Configure(EntityTypeBuilder<Developer> builder)
            {
                builder
                    .HasKey(x => x.Id);

                builder
                    .Property(x => x.Email)
                    .IsRequired();
                builder
                    .HasIndex(x => x.Email)
                    .IsUnique();

                builder
                    .Property(x => x.FirstName)
                    .IsRequired();

                builder
                    .Property(x => x.LastName)
                    .IsRequired();
            }
        }
    }
}
