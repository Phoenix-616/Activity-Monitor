using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models.Git
{
    class Statistic
    {
        public int id { get; set; } 
        public Developer developer { get; set; }
        public Repository repository { get; set; }
        public int activeDays { get; set; }
        public int codeSize { get; set; }
        public double churn { get; set; }
        public TimeSpan ageOfProject { get; set; }
        public double busNumber { get; set; }
    }
}
