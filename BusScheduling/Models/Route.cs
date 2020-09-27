using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BusScheduling.Models
{
    public class Route
    {
        [Key]
        [Required(ErrorMessage = "Enter Route no.")]
        public Int64 RouteNo { get; set; }

        [Required(ErrorMessage = "Enter Main Destination")]
        public string MainDestination { get; set; }

        [Required(ErrorMessage = "Enter Other Stands")]
        public string Stands { get; set; }
    }
}
