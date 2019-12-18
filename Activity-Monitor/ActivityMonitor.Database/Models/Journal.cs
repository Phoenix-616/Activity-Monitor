using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models
{
    class Journal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string CreatedOn{ get; set; }
    }
}
