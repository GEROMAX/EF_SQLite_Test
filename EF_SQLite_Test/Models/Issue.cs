using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_SQLite_Test.Models
{
    public enum IssueStatus
    {
        New,
        Process,
        Complete
    }

    public class Issue : EntityBase
    {
        [Index(IsUnique= true)]  
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Detail { get; set; }
        public DateTime RegistDate { get; set; }
        public IssueStatus Status { get; set; }
        public decimal Revision { get; set; }
    }
}
