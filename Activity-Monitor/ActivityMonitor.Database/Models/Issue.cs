using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityMonitor.Database.Models
{
    class Issue
    {
        public int Id { get; set; }
        public string TrackerName { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string AuthorName { get; set; }
        public string AuthorId { get; set; }
        public string CategoryName { get; set; }
        public string StartDate { get; set; }
        public string Created_on { get; set; }
        public string Updated_on { get; set; }
    }
}
