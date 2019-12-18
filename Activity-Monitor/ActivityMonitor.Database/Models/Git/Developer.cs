using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models.Git
{
    public class Developer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        ICollection<Repository> Repositories { get; set; }
        ICollection<Commit> Commits { get; set; }
    }
}
