using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusScheduling.Models
{
    public class Driver
    {
        [Key]
        [Required(ErrorMessage ="Enter Id")]
        public Int64 DriverId { get; set; }

        [Required(ErrorMessage = "Enter Name")]
        public string DriverName { get; set; }

        [Required(ErrorMessage = "Enter Contact")]
        public Int64 Contact { get; set; }

        [Required(ErrorMessage = "Enter Address")]
        public string Address { get; set; }
    }
}
