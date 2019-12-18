using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models.Git
{
    class Statistic
    {
        public int id;
        public Developer developer;
        public Repository repository; 
        public int activeDays;
        public int codeSize;
        public double churn;
        public TimeSpan ageOfProject;
        public double busNumber;
    }
}
