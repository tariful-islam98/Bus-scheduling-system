using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusScheduling.ViewModels
{
    public class UserViewModel
    {
        [Key]
        public Int64 UserId { get; set; }
        public string UserName { get; set; }
        public string Designation { get; set; }
    }
}
