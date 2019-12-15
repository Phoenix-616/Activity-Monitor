using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models
{
    class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Identifier { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string Created_on { get; set; }
        public string Updated_on { get; set; }
        public string Is_public { get; set; }
    }
}
