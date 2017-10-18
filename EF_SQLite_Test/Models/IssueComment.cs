using EF_SQLite_Test.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_SQLite_Test.Models
{
    public class IssueComment
    {
        [Key]
        [Index("HeightAndWeight", 1)]
        [Column(Order = 0)]
        public int IssueId { get; set; }
        [Key]
        [Index("HeightAndWeight", 2)]
        [Column(Order = 1)]
        public decimal CommentId { get; set; }
        [Required]
        [MaxByte(10)]
        public string Comment { get; set; }
    }
}
