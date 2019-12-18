using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models.Git
{
    public class File
    {
        public Repository Repository { get; set; }
        public string Name { get; set; }
        public ICollection<Commit> Commits { get; set; }
    }
}
