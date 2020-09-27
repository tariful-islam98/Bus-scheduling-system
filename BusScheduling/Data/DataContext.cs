using BusScheduling.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusScheduling.ViewModels;
//using System.Data.Entity;
namespace BusScheduling.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<BusSchedule> BusSchedule { get; set; }
        public DbSet<Route> Route { get; set; }
        public DbSet<Driver> Driver { get; set; }
        public DbSet<Notice> Notice { get; set; }
        public DbSet<BusScheduling.ViewModels.UserViewModel> UserViewModel { get; set; }
    }
}
