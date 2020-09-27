using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using BusScheduling.Models;
using BusScheduling.Data;
using BusScheduling.ViewModels;

namespace BusScheduling.Controllers
{
    public class HomeController : Controller
    {

        private readonly DataContext _db;
        public static Int64 StaticUserId=0;
        public static Int64 adminId=0;

        public HomeController(DataContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            var schedule =
                from sch in _db.BusSchedule
                select new
                {
                    BusNo = sch.BusNo,
                    Route = sch.Route,
                    DriverName = sch.DriverName,
                    Contact = sch.Contact,
                    Time = sch.Time
                };
            List<BusScheduleViewModel> busSchedule = new List<BusScheduleViewModel>();
            foreach (var item in schedule)
            {
                BusScheduleViewModel temp = new BusScheduleViewModel();
                temp.BusNo = item.BusNo;
                temp.Route = item.Route;
                temp.DriverName = item.DriverName;
                temp.Contact = item.Contact;
                temp.Time = item.Time;
                busSchedule.Add(temp);
            }
            return View(busSchedule);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Admin login form..
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Admin") != null && HttpContext.Session.GetString("Admin") != "Expired")
            {
                return RedirectToAction(nameof(UserList));
            }
            else
                return View();
        }

        //Admin login..
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Admin model)
        {
            var i = _db.Admin.Where(x => x.AdminId == model.AdminId && x.Password == model.Password).FirstOrDefault();
            if (i == null)
            {
                ModelState.AddModelError("LoginError", "Invalid Id or Password");
                return View();
            }
            else
            {
                HttpContext.Session.SetString("Admin", i.AdminName);
                adminId = i.AdminId;
                return RedirectToAction("UserList");
            }
        }

        //admin logout
        public IActionResult AdminLogout()
        {
            HttpContext.Session.SetString("Admin", "Expired");
            adminId = 0;
            return RedirectToAction(nameof(Login));
        }
        
        //Admin account
        public IActionResult AdminAccount()
        {
            if (HttpContext.Session.GetString("Admin") != null && HttpContext.Session.GetString("Admin") != "Expired")
            {
                var ad =
                    from adm in _db.Admin
                    where adm.AdminId == adminId
                    select adm;
                Admin admin = new Admin();
                admin.AdminId = ad.FirstOrDefault().AdminId;
                admin.AdminName = ad.FirstOrDefault().AdminName;
                admin.Password = ad.FirstOrDefault().Password;
                return View(admin);
            }
            return RedirectToAction(nameof(Login));
        }

        //Edit admin account..
        public IActionResult EditAdminAcc(int id)
        {
            if (HttpContext.Session.GetString("Admin") != null && HttpContext.Session.GetString("Admin") != "Expired")
            {
                var ad =
                    from adm in _db.Admin
                    where adm.AdminId == id
                    select adm;
                Admin admin = new Admin();
                admin.AdminId = ad.FirstOrDefault().AdminId;
                admin.AdminName = ad.FirstOrDefault().AdminName;
                admin.Password = ad.FirstOrDefault().Password;
                return View(admin);
            }
            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        public IActionResult EditAdminAcc(int id, Admin admin)
        {
            using (var db = _db)
            {
                Admin adm = new Admin();
                adm.AdminId = admin.AdminId;
                adm.AdminName = admin.AdminName;
                adm.Password = admin.Password;
                db.Admin.Update(adm);
                db.SaveChanges();
                return RedirectToAction(nameof(AdminAccount));
            }
        }

        //User login
        public IActionResult UserLogin()
        {
            if (HttpContext.Session.GetString("User") != null && HttpContext.Session.GetString("User") != "Expired")
            {
                return RedirectToAction(nameof(UserAccount));
            }
            else
                return View();
        }

        //User login..
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserLogin(User model)
        {
            var i = _db.User.Where(x => x.UserId == model.UserId && x.Password == model.Password).FirstOrDefault();
            if (i == null)
            {
                ModelState.AddModelError("UserLoginError", "Invalid Id or Password");
                return View();
            }
            else
            {
                HttpContext.Session.SetString("User", i.UserName);
                StaticUserId = i.UserId;
                return RedirectToAction("UserAccount");
            }
        }

        //User Account
        public IActionResult UserAccount()
        {
            if(HttpContext.Session.GetString("User")!= null && HttpContext.Session.GetString("User")!= "Expired")
            {
                var i =
                   from usr in _db.User
                   where usr.UserId == StaticUserId 
                   select usr;
                User user = new User();
                user.UserId = i.FirstOrDefault().UserId;
                user.UserName = i.FirstOrDefault().UserName;
                user.Password = i.FirstOrDefault().Password;
                user.Designation = i.FirstOrDefault().Designation;

                return View(user);
            }
            return RedirectToAction(nameof(UserLogin));
        }

        //User Logout
        public IActionResult UserLogout()
        {
            HttpContext.Session.SetString("User", "Expired");
            StaticUserId = 0;
            return RedirectToAction(nameof(UserLogin));
        }

        //User Information..
        public IActionResult UserList()
        {
            if(HttpContext.Session.GetString("Admin") != null && HttpContext.Session.GetString("Admin") != "Expired")
            {
                var userList =
                from usr in _db.User
                select new
                {
                    userId = usr.UserId,
                    userName = usr.UserName,
                    designation = usr.Designation
                };
                List<UserViewModel> userViewModels = new List<UserViewModel>();
                foreach (var item in userList)
                {
                    UserViewModel temp = new UserViewModel();
                    temp.UserId = item.userId;
                    temp.UserName = item.userName;
                    temp.Designation = item.designation;
                    userViewModels.Add(temp);
                }
                return View(userViewModels);
            }
            return RedirectToAction(nameof(Login));
        }

        //User registration form..
        public IActionResult UserRegistration()
        {
            if (HttpContext.Session.GetString("Admin") != null && HttpContext.Session.GetString("Admin") != "Expired")
            {
                return View();
            }
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public IActionResult UserRegistration(User user)
        {
            User usr = new User();

            usr.UserId = user.UserId;
            usr.UserName = user.UserName;
            usr.Password = user.Password;
            usr.Designation = user.Designation;
            var db = _db;
            db.User.Add(usr);
            db.SaveChanges();
            return RedirectToAction(nameof(UserList));
        }

        public IActionResult BusScheduleAdm()
        {
            if (HttpContext.Session.GetString("Admin") != null && HttpContext.Session.GetString("Admin") != "Expired")
            {
                var schedule =
                from sch in _db.BusSchedule
                select new
                {
                    BusNo = sch.BusNo,
                    Route = sch.Route,
                    DriverName = sch.DriverName,
                    Contact = sch.Contact,
                    Time = sch.Time
                };
                List<BusScheduleViewModel> busSchedule = new List<BusScheduleViewModel>();
                foreach (var item in schedule)
                {
                    BusScheduleViewModel temp = new BusScheduleViewModel();
                    temp.BusNo = item.BusNo;
                    temp.Route = item.Route;
                    temp.DriverName = item.DriverName;
                    temp.Contact = item.Contact;
                    temp.Time = item.Time;
                    busSchedule.Add(temp);
                }
                return View(busSchedule);
            }
            return RedirectToAction(nameof(Login));
        }

        public IActionResult CreateSchedule()
        {
            if (HttpContext.Session.GetString("Admin") != null && HttpContext.Session.GetString("Admin") != "Expired")
            {
                return View();
            }
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public IActionResult CreateSchedule(BusSchedule sch)
        {
            BusSchedule schedule = new BusSchedule();

            schedule.BusNo = sch.BusNo;
            schedule.Route = sch.Route;
            schedule.DriverName = sch.DriverName;
            schedule.Contact = sch.Contact;
            schedule.Time = sch.Time;
            var db = _db;
            db.BusSchedule.Add(schedule);
            db.SaveChanges();
            return RedirectToAction(nameof(BusScheduleAdm));
        }

        public IActionResult EditSchedule(int id)
        {
            if (HttpContext.Session.GetString("Admin") != null && HttpContext.Session.GetString("Admin") != "Expired")
            {
                var scheduleList =
                    from sc in _db.BusSchedule
                    where sc.BusNo == id
                    select sc;
                BusSchedule schedule = new BusSchedule();
                schedule.BusNo = scheduleList.FirstOrDefault().BusNo;
                schedule.Route = scheduleList.FirstOrDefault().Route;
                schedule.DriverName = scheduleList.FirstOrDefault().DriverName;
                schedule.Contact = scheduleList.FirstOrDefault().Contact;
                schedule.Time = scheduleList.FirstOrDefault().Time;
                return View(schedule);
            }
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public IActionResult EditSchedule(int id, BusSchedule sch)
        {
            using (var db = _db)
            {
                BusSchedule schedule = new BusSchedule();
                schedule.BusNo = sch.BusNo;
                schedule.Route = sch.Route;
                schedule.DriverName = sch.DriverName;
                schedule.Contact = sch.Contact;
                schedule.Time = sch.Time;
                db.BusSchedule.Update(schedule);
                db.SaveChanges();
                return RedirectToAction(nameof(BusScheduleAdm));
            }
        }

        public IActionResult DeleteSchedule(int id)
        {
            if (HttpContext.Session.GetString("Admin") != null && HttpContext.Session.GetString("Admin") != "Expired")
            {
                var sch =
                from sc in _db.BusSchedule
                where sc.BusNo == id
                select sc;
                BusSchedule schedule = new BusSchedule();
                schedule.BusNo = sch.FirstOrDefault().BusNo;
                schedule.Route = sch.FirstOrDefault().Route;
                schedule.DriverName = sch.FirstOrDefault().DriverName;
                schedule.Contact = sch.FirstOrDefault().Contact;
                schedule.Time = sch.FirstOrDefault().Time;
                return View(schedule);
            }
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public IActionResult DeleteSchedule(Int64 id)
        {
            using (var db = _db)
            {
                BusSchedule schedule =  _db.BusSchedule.Find(id);
                db.BusSchedule.Remove(schedule);
                db.SaveChanges();
                return RedirectToAction(nameof(BusScheduleAdm));
            }
        }

        //edit user account
        public IActionResult EditUser(int id)
        {
            if (HttpContext.Session.GetString("User") != null && HttpContext.Session.GetString("User") != "Expired")
            {
                var sch =
                    from sc in _db.User
                    where sc.UserId == id
                    select sc;
                User user = new User();
                user.UserId = sch.FirstOrDefault().UserId;
                user.UserName = sch.FirstOrDefault().UserName;
                user.Password = sch.FirstOrDefault().Password;
                user.Designation = sch.FirstOrDefault().Designation;
                return View(user);
            }
            else
                return RedirectToAction(nameof(UserLogin));
                
        }

        [HttpPost]
        public IActionResult EditUser(int id, User user)
        {
            if (HttpContext.Session.GetString("User") != null && HttpContext.Session.GetString("User") != "Expired")
            {
                using (var db = _db)
                {
                    User usr = new User();
                    usr.UserId = user.UserId;
                    usr.UserName = user.UserName;
                    usr.Password = user.Password;
                    usr.Designation = user.Designation;
                    db.User.Update(usr);
                    db.SaveChanges();
                    return RedirectToAction(nameof(UserAccount));
                }
            }
            else
                return RedirectToAction(nameof(UserLogin));
        }
        public IActionResult DeleteUser(int id)
        {
            if (HttpContext.Session.GetString("Admin") != null && HttpContext.Session.GetString("Admin") != "Expired")
            {
                var sch =
                from sc in _db.User
                where sc.UserId == id
                select sc;
                User user = new User();
                user.UserId = sch.FirstOrDefault().UserId;
                user.UserName = sch.FirstOrDefault().UserName;
                user.Password = sch.FirstOrDefault().Password;
                user.Designation = sch.FirstOrDefault().Designation;
                return View(user);
            }
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public IActionResult DeleteUser(Int64 id)
        {
            using (var db = _db)
            {
                User user =  _db.User.Find(id);
                db.User.Remove(user);
                db.SaveChanges();
                return RedirectToAction(nameof(UserList));
            }
        }

        public IActionResult DriverList()
        {
            if (HttpContext.Session.GetString("Admin") != null && HttpContext.Session.GetString("Admin") != "Expired")
            {
                var driverList =
                from dr in _db.Driver
                select new
                {
                    driverId = dr.DriverId,
                    driverName = dr.DriverName,
                    contact = dr.Contact,
                    address = dr.Address
                };
                List<Driver> drvlst = new List<Driver>();
                foreach (var item in driverList)
                {
                    Driver temp = new Driver();
                    temp.DriverId = item.driverId;
                    temp.DriverName = item.driverName;
                    temp.Contact = item.contact;
                    temp.Address = item.address;
                    drvlst.Add(temp);
                }
                return View(drvlst);
            }
            return RedirectToAction(nameof(Login));
        }

        public IActionResult AddDriver()
        {
            if (HttpContext.Session.GetString("Admin") != null && HttpContext.Session.GetString("Admin") != "Expired")
            {
                return View();
            }
            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        public IActionResult AddDriver(Driver driver)
        {
            Driver drv = new Driver();
            drv.DriverId = driver.DriverId;
            drv.DriverName = driver.DriverName;
            drv.Contact = driver.Contact;
            drv.Address = driver.Address;
            _db.Driver.Add(drv);
            _db.SaveChanges();
            return RedirectToAction(nameof(DriverList));
        }

        public IActionResult EditDriver(int id)
        {
            if (HttpContext.Session.GetString("Admin") != null && HttpContext.Session.GetString("Admin") != "Expired")
            {
                var dvr =
                    from dv in _db.Driver
                    where dv.DriverId == id
                    select dv;
                Driver driver = new Driver();
                driver.DriverId = dvr.FirstOrDefault().DriverId;
                driver.DriverName = dvr.FirstOrDefault().DriverName;
                driver.Contact = dvr.FirstOrDefault().Contact;
                driver.Address = dvr.FirstOrDefault().Address;
                return View(driver);
            }
            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        public IActionResult EditDriver(int id, Driver driver)
        {
            using(var db = _db)
            {
                Driver drv = new Driver();
                drv.DriverId = driver.DriverId;
                drv.DriverName = driver.DriverName;
                drv.Contact = driver.Contact;
                drv.Address = driver.Address;
                db.Driver.Update(drv);
                db.SaveChanges();
                return RedirectToAction(nameof(DriverList));
            }
        }

        public IActionResult DeleteDriver(int id)
        {
            if (HttpContext.Session.GetString("Admin") != null && HttpContext.Session.GetString("Admin") != "Expired")
            {
                var dvr =
                    from dv in _db.Driver
                    where dv.DriverId == id
                    select dv;
                Driver driver = new Driver();
                driver.DriverId = dvr.FirstOrDefault().DriverId;
                driver.DriverName = dvr.FirstOrDefault().DriverName;
                driver.Contact = dvr.FirstOrDefault().Contact;
                driver.Address = dvr.FirstOrDefault().Address;
                return View(driver);
            }
            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        public IActionResult DeleteDriver(Int64 id)
        {
            using(var db = _db)
            {
                Driver driver = db.Driver.Find(id);
                db.Driver.Remove(driver);
                db.SaveChanges();
                return RedirectToAction(nameof(DriverList));
            }
        }

        public IActionResult Routes(string route)
        {
            var rt =
                from r in _db.Route
                where r.MainDestination == route
                select r;
            Route stnd = new Route();
            stnd.RouteNo = rt.FirstOrDefault().RouteNo;
            stnd.MainDestination = rt.FirstOrDefault().MainDestination;
            stnd.Stands = rt.FirstOrDefault().Stands;
            return View(stnd);
        }
        public IActionResult RoutesAd(string route)
        {
            var rt =
                from r in _db.Route
                join s in _db.BusSchedule on r.MainDestination equals s.Route
                where s.Route == route
                select new { rtno = r.RouteNo, md = r.MainDestination, stn = r.Stands };
            Route stnd = new Route();
            stnd.RouteNo = rt.FirstOrDefault().rtno;
            stnd.MainDestination = rt.FirstOrDefault().md;
            stnd.Stands = rt.FirstOrDefault().stn;
            return View(stnd);
        }

        public IActionResult CreateRoute()
        {
            if (HttpContext.Session.GetString("Admin") != null && HttpContext.Session.GetString("Admin") != "Expired")
            {
                return View();
            }
            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        public IActionResult CreateRoute(Route route)
        {
            Route rt = new Route();
            rt.RouteNo = route.RouteNo;
            rt.MainDestination = route.MainDestination;
            rt.Stands = route.Stands;
            _db.Route.Add(rt);
            _db.SaveChanges();
            return RedirectToAction(nameof(Routes));
        }

        public IActionResult EditRoute(int id)
        {
            if (HttpContext.Session.GetString("Admin") != null && HttpContext.Session.GetString("Admin") != "Expired")
            {
                var er =
                    from rt in _db.Route
                    where rt.RouteNo == id
                    select rt;
                Route route = new Route();
                route.RouteNo = er.FirstOrDefault().RouteNo;
                route.MainDestination = er.FirstOrDefault().MainDestination;
                route.Stands = er.FirstOrDefault().Stands;
                return View(route);
            }
            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        public IActionResult EditRoute(int id, Route route)
        {
            Route rt = new Route();
            rt.RouteNo = route.RouteNo;
            rt.MainDestination = route.MainDestination;
            rt.Stands = route.Stands;
            _db.Route.Update(rt);
            _db.SaveChanges();
            return RedirectToAction(nameof(RoutesAd));
        }
    }
}
