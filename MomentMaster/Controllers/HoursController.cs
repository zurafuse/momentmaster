using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MomentMaster.Models;

namespace MomentMaster.Controllers
{
    public class HoursController : Controller
    {
        private readonly HoursContext _context;

        public HoursController(HoursContext context)
        {
            _context = context;
        }

        public bool IsProjectFound(int id)
        {
            var projectList = _context.TimeObject.ToList();
            bool projectIsFound = false;
            foreach (TimeObject project in projectList)
            {
                if (project.Project_Id == id)
                {
                    projectIsFound = true;
                }
            }
            return projectIsFound;
        }
        public void SetProjectDescription(int id)
        {
            var projectList = _context.TimeObject.ToList();
            foreach (TimeObject project in projectList)
            {
                if (project.Project_Id == id)
                {
                    ViewBag.Name = project.Description;
                }
            }
        }

        // GET: Hours
        public async Task<IActionResult> Index()
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            return View(await _context.Hours.ToListAsync());
        }

        //GET: TimeSheets
        public async Task<IActionResult> TimeSheets()
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            var userList = await _context.User.ToListAsync();
            var hourList = await _context.Hours.ToListAsync();
            for (var i = 0; i < userList.Count; i++)
            {
                foreach (Hours hour in hourList)
                {
                    if (hour.User_Id == userList[i].User_Id)
                    {
                        userList[i].Hours += hour.GetHours();
                        hour.User = userList[i];
                    }
                }
            }
            
            ViewBag.Users = userList;

            return View(hourList);
        }

        //POST: TimeSheet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TimeSheet(int id, double hours, DateTime StartDate, DateTime EndDate)
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            ViewBag.User = new User();
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.DateMessage = " ";
            if (StartDate > EndDate)
            {
                ViewBag.DateMessage = "WARNING: the end date range you selected (" + EndDate + ") is earlier than the starting date range (" + StartDate + "). Please go back and try again.";
            }
            var userList = _context.User.ToList();
            foreach (User thisUser in userList)
            {
                if (thisUser.User_Id == id)
                {
                    ViewBag.User = thisUser;
                }
            }
            ViewBag.User.Hours = hours;
            var hourList = await _context.Hours.ToListAsync();
            var thisHourList = new List<Hours>();
            foreach (Hours hour in hourList)
            {
                if (hour.User_Id == id)
                {
                    if (hour.StartTime >= Convert.ToDateTime(StartDate) && hour.EndTime <= Convert.ToDateTime(EndDate))
                    {
                        thisHourList.Add(hour);
                    }
                }
            }
            return View(thisHourList);
        }

        // GET: Hours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            if (id == null)
            {
                return NotFound();
            }

            var hours = await _context.Hours
                .FirstOrDefaultAsync(m => m.Hour_Id == id);
            if (hours == null)
            {
                return NotFound();
            }
            var userList = _context.User.ToList();
            var projectList = _context.TimeObject.ToList();
            foreach (User thisUser in userList)
            {
                if (hours.User_Id == thisUser.User_Id)
                {
                    hours.User = thisUser;
                }
            }
            foreach (TimeObject project in projectList)
            {
                if (project.Project_Id == hours.Project_Id)
                {
                    ViewBag.project = project;
                }
            }
            SetProjectDescription(hours.Project_Id);
            ViewBag.isProjectFound = IsProjectFound(hours.Project_Id);
            return View(hours);
        }

        // GET: Hours/Create
        public IActionResult Create(int id)
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            ViewBag.id = id;
            var projectList = _context.TimeObject.ToList();
            SetProjectDescription(id);
            ViewBag.Users = _context.User;
            return View();
        }

        // POST: Hours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Hour_Id,Project_Id,User_Id,Description,StartTime,EndTime")] Hours hours)
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            if (ModelState.IsValid)
            {
                _context.Add(hours);
                await _context.SaveChangesAsync();
                return Redirect("/TimeObjects/Details/" + hours.Project_Id);
            }
            return View(hours);
        }

        // GET: Hours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            if (id == null)
            {
                return NotFound();
            }

            var hours = await _context.Hours.FindAsync(id);
            if (hours == null)
            {
                return NotFound();
            }

            var projectList = _context.TimeObject.ToList();
            ViewBag.project = new TimeObject();
            foreach (TimeObject project in projectList)
            {
                if (project.Project_Id == hours.Project_Id)
                {
                    ViewBag.project = project;
                }
            }
            ViewBag.Users = _context.User;
            ViewBag.isProjectFound = IsProjectFound(hours.Project_Id);
            return View(hours);
        }

        // POST: Hours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Hour_Id,Project_Id,User_Id,Description,StartTime,EndTime")] Hours hours)
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            if (id != hours.Hour_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hours);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoursExists(hours.Hour_Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (IsProjectFound(hours.Project_Id) == true)
                {
                    return Redirect("/TimeObjects/Details/" + hours.Project_Id);
                }
                else
                {
                    return Redirect("/Hours/");
                }
            }
            return View(hours);
        }

        // GET: Hours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            if (id == null)
            {
                return NotFound();
            }

            var hours = await _context.Hours
                .FirstOrDefaultAsync(m => m.Hour_Id == id);
            if (hours == null)
            {
                return NotFound();
            }
            ViewBag.isProjectFound = IsProjectFound(hours.Project_Id);

                return View(hours);
        }

        // POST: Hours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            var hours = await _context.Hours.FindAsync(id);
            _context.Hours.Remove(hours);
            await _context.SaveChangesAsync();
            if (IsProjectFound(hours.Project_Id) == true)
            {
                return Redirect("/TimeObjects/Details/" + hours.Project_Id);
            }
            else
            {
                return Redirect("/Hours/");
            }
        }

        private bool HoursExists(int id)
        {
            return _context.Hours.Any(e => e.Hour_Id == id);
        }
    }
}
