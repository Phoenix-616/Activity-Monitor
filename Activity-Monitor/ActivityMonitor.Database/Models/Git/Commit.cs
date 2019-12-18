using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models.Git
{
    public class Commit
    {
        public Developer Developer { get; set; }
        public Repository Repository { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public ICollection<File> Files { get; set; }
        public int Created { get; set; }
        public int Deleted { get; set; }
    }
}
