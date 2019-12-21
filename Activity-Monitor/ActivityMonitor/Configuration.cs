using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor
{
    class Configuration
    {
        public string PMTUri { get; set; }
        public string PMTLogin { get; set; }
        public string PMTPassword { get; set; }
        public List<string> PMTProjects { get; set; }

    }
}
