using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models.Git
{
    class Developer
    {
        public int id;
        public string login;
        public string firstName;
        public string lastName;
        public ICollection<Repository> repositories;
    }
}
