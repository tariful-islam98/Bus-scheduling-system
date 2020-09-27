using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BusScheduling.Models
{
    public class User
    {
        [Key]
        [Required(ErrorMessage ="Enter User Id")]
        public Int64 UserId { get; set; }
        [Required(ErrorMessage = "Enter User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Enter Designation")]
        public string Designation { get; set; }
    }
}
