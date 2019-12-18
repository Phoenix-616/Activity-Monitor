using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models.Git
{
    public class Repository
    {
        public string OwnersLogin { get; set; }
        public string Name { get; set; }
        public ICollection<Developer> Developers { get; set; }

        public ICollection<Commit> Commits { get; set; }
        public ICollection<File> Files { get; set; }
    }
}
