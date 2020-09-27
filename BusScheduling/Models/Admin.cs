using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusScheduling.Models
{
    public class Admin
    {
        [Key]
        [Required(ErrorMessage ="Enter ID")]
        public Int64 AdminId { get; set; }

        [Required(ErrorMessage ="Enter Name")]
        public string AdminName { get; set; }

        [Required(ErrorMessage ="Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
