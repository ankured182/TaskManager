using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [Authorize]
    public class TaskProcessController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskProcessController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TaskProcess
        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.TaskMains.Include(t => t.Modules).Include(t => t.Projects);
            //return View(await applicationDbContext.ToListAsync());

            //.Include(t => t.Projects).OrderByDescending(t => t.Id);
            string user = User.Identity.Name;
            if (user != "reza")
            {
                var m = from cust in _context.TaskMains
                            //where cust.CreateBy == user
                        where cust.AssignedTo == user
                        && cust.CurrentStatus != "Completed"
                        select cust;

                m = m.Include(t => t.Modules).Include(t => t.Projects).OrderByDescending(t => t.ProjectId);
                return View(await m.ToListAsync());

            }
            else
            {
                var m = from cust in _context.TaskMains
                        where cust.CurrentStatus!= "Completed"
                        select cust;
                m = m.Include(t => t.Modules).Include(t => t.Projects).OrderByDescending(t => t.ProjectId);
                return View(await m.ToListAsync());
            }

        }


        public async Task<IActionResult> IndexC()
        {
            //var applicationDbContext = _context.TaskMains.Include(t => t.Modules).Include(t => t.Projects);
            //return View(await applicationDbContext.ToListAsync());

            //.Include(t => t.Projects).OrderByDescending(t => t.Id);
            string user = User.Identity.Name;
            if (user != "reza")
            {
                var m = from cust in _context.TaskMains
                            //where cust.CreateBy == user
                        where cust.AssignedTo == user
                        && cust.CurrentStatus == "Completed"
                        select cust;

                m = m.Include(t => t.Modules).Include(t => t.Projects).OrderByDescending(t => t.Id);
                return View(await m.ToListAsync());

            }
            else
            {
                var m = from cust in _context.TaskMains
                        where cust.CurrentStatus == "Completed"
                        select cust;
                m = m.Include(t => t.Modules).Include(t => t.Projects).OrderByDescending(t => t.Id);
                return View(await m.ToListAsync());
            }

        }


        // GET: TaskProcess/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TaskMains == null)
            {
                return NotFound();
            }

            var taskMain = await _context.TaskMains
                .Include(t => t.Modules)
                .Include(t => t.Projects)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskMain == null)
            {
                return NotFound();
            }

            return View(taskMain);
        }

        // GET: TaskProcess/Create
        public IActionResult Create()
        {
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName");
            return View();
        }

        // POST: TaskProcess/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TaskName,ProjectId,ModuleId,CreateBy,AssignedTo,CurrentStatus,DateCreated,ActualDateStarted,ActualDateEnded")] TaskMain taskMain)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskMain);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName", taskMain.ModuleId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName", taskMain.ProjectId);

            return View(taskMain);
        }

        // GET: TaskProcess/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TaskMains == null)
            {
                return NotFound();
            }

            var taskMain = await _context.TaskMains.FindAsync(id);
            if (taskMain == null)
            {
                return NotFound();
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName", taskMain.ModuleId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName", taskMain.ProjectId);

          //  return RedirectToAction(nameof(Index));

           // return RedirectToAction("Index",new {Controller="TaskMains"});

            return View(taskMain);
        }

        // POST: TaskProcess/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TaskName,TaskDescrp,ProjectId,ModuleId,CreateBy,AssignedTo,CurrentStatus,DateCreated,IntendedStartDate,DurationHrs,ActualDateStarted,ActualDateEnded")] TaskMain taskMain)
        {
            if (id != taskMain.Id)
            {
                return NotFound();
            }

            if(taskMain.ActualDateStarted!=null)
            {
                taskMain.CurrentStatus = "Started";
            }
            else
            {
                taskMain.CurrentStatus = "Created";
            }


            if (taskMain.ActualDateEnded != null)
            {
                taskMain.CurrentStatus = "Completed";
            }
            else
            {
                if (taskMain.ActualDateStarted != null)
                {
                    taskMain.CurrentStatus = "Started";
                }
                else
                {
                    taskMain.CurrentStatus = "Created";
                }

            }

            if (taskMain.DurationHrs == null)
            {
                taskMain.DurationHrs= 0;
            }

            //if (ModelState.IsValid)
            //{
            try
            {
                var record6 = _context.TaskMains.Where(record => record.Id == id).FirstOrDefault();

                record6.ActualDateStarted = taskMain.ActualDateStarted;
                record6.ActualDateEnded = taskMain.ActualDateEnded;
                record6.CurrentStatus = taskMain.CurrentStatus;
                record6.DurationHrs=taskMain.DurationHrs;

                _context.Update(record6);
                _context.SaveChanges();


                //_context.Update(taskMain);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskMainExists(taskMain.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            // return RedirectToAction(nameof(Index));

            return RedirectToAction("Index", new { Controller = "TaskMains" });

            //}
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName", taskMain.ModuleId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName", taskMain.ProjectId);
            return View(taskMain);
        }

        // GET: TaskProcess/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TaskMains == null)
            {
                return NotFound();
            }

            var taskMain = await _context.TaskMains
                .Include(t => t.Modules)
                .Include(t => t.Projects)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskMain == null)
            {
                return NotFound();
            }

            return View(taskMain);
        }

        // POST: TaskProcess/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TaskMains == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TaskMains'  is null.");
            }
            var taskMain = await _context.TaskMains.FindAsync(id);
            if (taskMain != null)
            {
                _context.TaskMains.Remove(taskMain);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskMainExists(int id)
        {
          return (_context.TaskMains?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
