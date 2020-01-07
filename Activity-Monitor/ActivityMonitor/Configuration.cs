using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor
{
    public class RepositoryAttribute
    {
        public string Owner { get; set; }
        public string Name { get; set; }
    }
    class Configuration
    {
        public string PMTUri { get; set; }
        public string PMTLogin { get; set; }
        public string PMTPassword { get; set; }
        public List<string> PMTProjects { get; set; }
        public string GitLogin { get; set; }
        public string GitPassword { get; set; }
        public RepositoryAttribute[] GitRepositories { get; set; }

    }
}
