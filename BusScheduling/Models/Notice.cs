using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BusScheduling.Models
{
    public class Notice
    {
        [Key]
        [Required(ErrorMessage ="Enter Id")]
        public Int64 NoticeId { get; set; }

        [Required(ErrorMessage = "Write Notice")]
        public string NoticeContent { get; set; }

        [Required(ErrorMessage = "Enter Time")]
        public DateTime Time { get; set; }
    }
}
