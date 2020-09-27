using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BusScheduling.ViewModels
{
    public class BusScheduleViewModel
    {
        [Key]
        public Int64 BusNo { get; set; }
        public string Route { get; set; }
        public string DriverName { get; set; }
        public Int64 Contact { get; set; }
        public string Time { get; set; }
    }
}
