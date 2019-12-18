using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models.Git
{
    class Repository
    {
        public int id;
        public string owner;
        public string name;
        public ICollection<Developer> developers;
    }
}
