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
    public class CommentsController : Controller
    {
        private readonly CommentContext _context;

        public CommentsController(CommentContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            return View(await _context.Comment.ToListAsync());
        }

        // GET: Comments/Details/5
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

            var comment = await _context.Comment
                .FirstOrDefaultAsync(m => m.Comment_Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            var projectList = _context.TimeObject.ToList();
            foreach (TimeObject project in projectList)
            {
                if (project.Project_Id == comment.Project_Id)
                {
                    ViewBag.Name = project.Description;
                }
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create(int id)
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            ViewBag.id = id;
            var projectList = _context.TimeObject.ToList();
            foreach (TimeObject project in projectList)
            {
                if (project.Project_Id == id)
                {
                    ViewBag.Name = project.Description;
                }
            }
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Comment_Id,Project_Id,Content")] Comment comment)
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return Redirect("/TimeObjects/Details/" + comment.Project_Id);
            }
            return View(comment);
        }

        // GET: Comments/Edit/5
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

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            var projectList = _context.TimeObject.ToList();
            foreach (TimeObject project in projectList)
            {
                if (project.Project_Id == comment.Project_Id)
                {
                    ViewBag.Name = project.Description;
                }
            }

            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Comment_Id,Project_Id,Content")] Comment comment)
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            if (id != comment.Comment_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Comment_Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/TimeObjects/Details/" + comment.Project_Id);
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
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

            var comment = await _context.Comment
                .FirstOrDefaultAsync(m => m.Comment_Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            var projectList = _context.TimeObject.ToList();
            foreach (TimeObject project in projectList)
            {
                if (project.Project_Id == comment.Project_Id)
                {
                    ViewBag.Name = project.Description;
                }
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (LoginStatus.UserValidated(HttpContext.Session.GetString("Key")) == false)
            {
                return Redirect("/Home/Index/");
            }
            var comment = await _context.Comment.FindAsync(id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return Redirect("/TimeObjects/Details/" + comment.Project_Id);
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.Comment_Id == id);
        }
    }
}
