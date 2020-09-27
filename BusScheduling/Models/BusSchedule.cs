using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusScheduling.Models
{
    public class BusSchedule
    {
        [Key]
        public Int64 BusNo { get; set; }
        public string Route { get; set; }
        public string DriverName { get; set; }
        public Int64 Contact { get; set; }
        public string Time { get; set; }
    }
}
