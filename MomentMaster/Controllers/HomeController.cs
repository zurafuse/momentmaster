using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MomentMaster.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace MomentMaster.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserContext _context;

        public HomeController(UserContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BadLogin()
        {
            return View();
        }

        public IActionResult Validate(string uname, string password)
        {
            bool goodLogin = false;
            var userList = _context.User.ToList();
            foreach(User thisUser in userList)
            {
                if (thisUser.UserName == uname && thisUser.Password == password)
                {
                    goodLogin = true;
                    HttpContext.Session.SetString("Key", HttpContext.Session.Id + "_" + 
                        DateTime.Now + "_" + thisUser.User_Id + thisUser.UserName);
                    LoginStatus.Keys.Add(HttpContext.Session.GetString("Key"));
                    break;
                }
            }
            if (goodLogin)
            {
                return Redirect("/TimeObjects/Index/");
            }
            return RedirectToAction(nameof(BadLogin));
        }

        public IActionResult Logoff(string uname, string psw)
        {
            LoginStatus.LogOff(HttpContext.Session.GetString("Key"));
            return RedirectToAction(nameof(Index));
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
    }
}
