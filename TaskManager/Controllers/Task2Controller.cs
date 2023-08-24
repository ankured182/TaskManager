using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    public class Task2Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public Task2Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Task2
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TaskMains.Include(t => t.Modules).Include(t => t.Projects);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Task2/Details/5
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

        // GET: Task2/Create
        public IActionResult Create()
        {
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName");
            return View();
        }

        // POST: Task2/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TaskName,TaskDescrp,ProjectId,ModuleId,CreateBy,AssignedTo,CurrentStatus,DateCreated,IntendedStartDate,DurationHrs,ActualDateStarted,ActualDateEnded")] TaskMain taskMain)
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

        // GET: Task2/Edit/5
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
            return View(taskMain);
        }

        // POST: Task2/Edit/5
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskMain);
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName", taskMain.ModuleId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName", taskMain.ProjectId);
            return View(taskMain);
        }

        // GET: Task2/Delete/5
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

        // POST: Task2/Delete/5
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
