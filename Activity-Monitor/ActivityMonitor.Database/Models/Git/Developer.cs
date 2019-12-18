using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models.Git
{
    class Developer
    {
        public int id { get; set; }
        public string login { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public ICollection<Repository> repositories { get; set; }
    }
}
