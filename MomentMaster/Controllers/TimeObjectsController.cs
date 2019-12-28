using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MomentMaster.Models;

namespace MomentMaster.Controllers
{
    public class TimeObjectsController : Controller
    {
        private readonly TimeObjectContext _context;

        public TimeObjectsController(TimeObjectContext context)
        {
            _context = context;
        }

        public bool IsProjectFound(int id)
        {
            try
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
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }
        }

        //Populate TimeObject with Comments and Hours, and assign the User.
        public TimeObject UpdateTimeObject(TimeObject thisTimeObject) 
        {
            try
            {
                //Populate Comments
                List<Comment> commentList = _context.Comment.ToList();
                //Set the User
                List<User> userList = _context.User.ToList();
                for (var i = 0; i < userList.Count; i++)
                {
                    if (userList[i].User_Id == thisTimeObject.User_Id)
                    {
                        thisTimeObject.SetUser(userList[i]);
                    }
                }
                //Populate Hours
                thisTimeObject.PopulateHours(_context.User.ToList(), _context.Hours.ToList(), _context.TimeObject.ToList(), thisTimeObject.Project_Id);

                //Populate Subtasks
                for (var i = 0; i < _context.TimeObject.ToList().Count; i++)
                {
                    TimeObject subTask = _context.TimeObject.ToList()[i];
                    if (subTask.Parent_Id == thisTimeObject.Project_Id)
                    {
                        thisTimeObject.AddSubTask(subTask);
                    }
                }
                return thisTimeObject;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return new TimeObject();
            }
        }

        // GET: TimeObjects
        public async Task<IActionResult> Index()
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            List<TimeObject> timeObjectList = await _context.TimeObject.ToListAsync();
            List<TimeObject> timeObjects = new List<TimeObject>();
            for (var i = 0; i < timeObjectList.Count; i++)
            {
                if (timeObjectList[i].Parent_Id == -1)
                {
                    timeObjects.Add(timeObjectList[i]);
                }
            }

            //populate the users for each time object.
            for (var i = 0; i < timeObjects.Count; i++) {
                timeObjects[i] = UpdateTimeObject(timeObjects[i]);
            }
            return View(timeObjects);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(string searchText)
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            if (searchText == null)
            {
                searchText = "";
            }
            List<TimeObject> timeObjectList = await _context.TimeObject.ToListAsync();
            List<TimeObject> timeObjects = new List<TimeObject>();
            for (var i = 0; i < timeObjectList.Count; i++)
            {
                if (timeObjectList[i].Description.ToUpper().Contains(searchText.ToUpper()) ||
                    timeObjectList[i].Project_Id.ToString() == searchText)
                {
                    timeObjects.Add(timeObjectList[i]);
                }
            }

            //populate the users for each time object.
            for (var i = 0; i < timeObjects.Count; i++)
            {
                timeObjects[i] = UpdateTimeObject(timeObjects[i]);
            }

            return View(timeObjects);
        }

        // GET: TimeObjects/Details/5
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

            var timeObject = await _context.TimeObject
                .FirstOrDefaultAsync(m => m.Project_Id == id);

            timeObject = UpdateTimeObject(timeObject);

            if (timeObject == null)
            {
                return NotFound();
            }
            ViewBag.isProjectFound = IsProjectFound(timeObject.Parent_Id);

            return View(timeObject);
        }

        // GET: TimeObjects/Create
        public IActionResult Create(int id)
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            var userList = _context.User.ToList();
            ViewBag.id = id;
            ViewBag.Users = userList;
            return View();
        }
        
        // POST: TimeObjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Project_Id,Parent_Id,Type,Tier,Description,User_Id,StartDate,OriginalEstimate,HoursBudgeted,Status")] TimeObject timeObject)
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            if (ModelState.IsValid)
            {
                timeObject.UpdateTier();
                _context.Add(timeObject);
                await _context.SaveChangesAsync();
                if (timeObject.Parent_Id == -1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Details", new { id = timeObject.Parent_Id });
                }
            }
            timeObject = UpdateTimeObject(timeObject);
            return View(timeObject);
        }

        // GET: TimeObjects/Edit/5
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

            var timeObject = await _context.TimeObject.FindAsync(id);
            timeObject = UpdateTimeObject(timeObject);
            if (timeObject == null)
            {
                return NotFound();
            }
            var userList = _context.User.ToList();
            ViewBag.Users = userList;
            ViewBag.isProjectFound = IsProjectFound(timeObject.Parent_Id);
            return View(timeObject);
        }

        // POST: TimeObjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Project_Id,Parent_Id,Type,Tier,Description,User_Id,StartDate,OriginalEstimate,HoursBudgeted,Status")] TimeObject timeObject)
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            if (id != timeObject.Project_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    timeObject.UpdateTier();
                    _context.Update(timeObject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeObjectExists(timeObject.Project_Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (timeObject.Parent_Id == -1 || IsProjectFound(timeObject.Parent_Id) == false)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Details", new { id = timeObject.Parent_Id });
                }
            }
            return View(timeObject);
        }

        // GET: TimeObjects/Delete/5
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

            var timeObject = await _context.TimeObject
                .FirstOrDefaultAsync(m => m.Project_Id == id);
            if (timeObject == null)
            {
                return NotFound();
            }
            ViewBag.isProjectFound = IsProjectFound(timeObject.Parent_Id);
            return View(timeObject);
        }

        // POST: TimeObjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            var timeObject = await _context.TimeObject.FindAsync(id);
            _context.TimeObject.Remove(timeObject);
            await _context.SaveChangesAsync();
            if (timeObject.Parent_Id == -1 || IsProjectFound(timeObject.Parent_Id) == false)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Details", new { id = timeObject.Parent_Id });
            }
        }

        private bool TimeObjectExists(int id)
        {
            return _context.TimeObject.Any(e => e.Project_Id == id);
        }
    }
}
