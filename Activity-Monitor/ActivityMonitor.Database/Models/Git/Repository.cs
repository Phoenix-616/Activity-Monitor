using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models.Git
{
    class Repository
    {
        public int id { get; set; }
        public string owner { get; set; }
        public string name { get; set; }
        public ICollection<Developer> developers { get; set; }
    }
}
