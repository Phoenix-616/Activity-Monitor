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
        public string Priority { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
        public int AssignedTo { get; set; }
        public string CategoryName { get; set; }
        public string Subject { get; set; }
        public string StartDate { get; set; }
        public string DueDate { get; set; }
    }
}
