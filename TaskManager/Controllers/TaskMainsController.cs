using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;


namespace TaskManager.Controllers
{
    [Authorize]
    public class TaskMainsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;
        Utilities u = new Utilities();
        public TaskMainsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TaskMains
        public async Task<IActionResult> Index()
        {
           
            //.Include(t => t.Projects).OrderByDescending(t => t.Id);
            string user = User.Identity.Name;
            if (user!="reza")
            {
               var m= from cust in _context.TaskMains
                   //where cust.CreateBy == user
                   where cust.AssignedTo == user
                   && cust.CurrentStatus != "Completed"
                      select cust;

                m = m.Include(t => t.Modules).Include(t => t.Projects).Include(t => t.TaskPriorities)
                     
                     //.OrderByDescending(t => t.Projects.ProjectId)
                    .OrderByDescending(t => t.TaskPriorities.PriorityId).ThenBy(t=>t.Projects.ProjectId);
                //.OrderByDescending(t => t.Id);
                return View(await m.ToListAsync());

            }
            else
            {
                var m = from cust in _context.TaskMains 
                        where  cust.CurrentStatus != "Completed"
                        select cust;
                m = m.Include(t => t.Modules).Include(t => t.Projects).OrderByDescending(t => t.Id);
                return View(await m.ToListAsync());
            }           
        }


        public async Task<IActionResult> GetRptTask()
        {       
            string df = DateTime.Now.ToString("MM")+"/01/"+ DateTime.Now.ToString("yyyy");
            string dt = DateTime.Now.ToString("MM/dd/yyyy");

            var m = _context.RptTasks
    .FromSqlRaw("EXEC RptUserTaskDatewise '" + df + "','"+dt+"'")
    .ToList();



            //var m = from cust in _context.RptTasks

            //        select cust;

            return View(m);

        }
        [HttpPost]
        public async Task<IActionResult> GetRptTask(string df,string dt)
        {
            //df = "06/13/2022";
            //dt = "06/21/2022";

            var m = _context.RptTasks
             .FromSqlRaw("EXEC RptUserTaskDatewise '"+df+"','"+dt+"'")
              .ToList();
 


            //var m = from cust in _context.RptTasks

            //        select cust;

            return View(m);

        }



        // GET: TaskMains/Details/5
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

        // GET: TaskMains/Create

        
       
        public IActionResult Create()
        {
            List<MyUser> s = new List<MyUser>();
         
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName");
            ViewData["PriorityId"] = new SelectList(_context.TaskPriorities, "PriorityId", "PriorityName");

            //if (User.Identity.Name == "reza" || User.Identity.Name == "rashed")
            //{
            foreach (var module in _context.Users.ToList())
                {
                    MyUser user = new MyUser();
                    user.Id = module.UserName;
                    user.UserName = module.UserName;

                    s.Add(user);

                }
            //}
            //else
            //{
            //    MyUser user = new MyUser();
            //    user.Id = User.Identity.Name;
            //    user.UserName = User.Identity.Name;

            //    s.Add(user);
            //}

            ViewData["UserId"] = new SelectList(s, "Id", "UserName");

            return View();
        }

        // POST: TaskMains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TaskName,ProjectId,ModuleId,PriorityId,CreateBy,AssignedTo,CurrentStatus,DateCreated,ActualDateStarted,ActualDateEnded,DurationHrs,IntendedStartDate,TaskDescrp")] TaskMain taskMain)
        {
            //if (ModelState.IsValid)
            //{
            taskMain.CurrentStatus = "Created";
            taskMain.DateCreated = DateTime.Now;
            taskMain.CreateBy = User.Identity.Name;

            if(taskMain.DurationHrs==null)
            {
                taskMain.DurationHrs =0;
            }

            _context.Add(taskMain);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName", taskMain.ModuleId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName", taskMain.ProjectId);
            ViewData["PriorityId"] = new SelectList(_context.TaskPriorities, "PriorityId", "PriorityName", taskMain.PriorityId);

            // ViewData["UserId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName", taskMain.ProjectId);

            return View(taskMain);
        }

        // GET: TaskMains/Edit/5
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

            //var d = _context.Projects;
            //d = d.Select(x => x.Id);

            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName", taskMain.ProjectId);

            ViewData["PriorityId"] = new SelectList(_context.TaskPriorities, "PriorityId", "PriorityName", taskMain.PriorityId);

            List<MyUser> s = new List<MyUser>();

            foreach (var module in _context.Users.ToList())
            {
                MyUser user = new MyUser();
                user.Id = module.UserName;
                user.UserName = module.UserName;
                s.Add(user);
            }

            ViewData["UserId"] = new SelectList(s, "Id", "UserName");

            return View(taskMain);
        }

        // POST: TaskMains/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("Id,TaskName,TaskDescrp,ProjectId,ModuleId,PriorityId,CreateBy,AssignedTo,CurrentStatus,DateCreated,IntendedStartDate,DurationHrs,ActualDateStarted,ActualDateEnded")] TaskMain taskMain)
        {
            if (id != taskMain.Id)
            {
                return NotFound();
            }

         
            //if (ModelState.IsValid)
            //{
            try
                {
                var record6 = _context.TaskMains.Where(record => record.Id == id).FirstOrDefault();

                record6.TaskName = taskMain.TaskName;
                record6.TaskDescrp = taskMain.TaskDescrp;
                record6.ProjectId = taskMain.ProjectId;
                record6.ModuleId = taskMain.ModuleId;
                record6.PriorityId = taskMain.PriorityId;

                record6.CreateBy = taskMain.CreateBy;
                record6.AssignedTo = taskMain.AssignedTo;
                record6.DurationHrs = taskMain.DurationHrs;
                record6.IntendedStartDate = taskMain.IntendedStartDate;

                _context.Update(record6);
                _context.SaveChanges();

               // _context.Update(taskMain);
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
           // }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName", taskMain.ModuleId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName", taskMain.ProjectId);
            ViewData["PriorityId"] = new SelectList(_context.Projects, "PriorityId", "PriorityName", taskMain.PriorityId);

            return View(taskMain);
        }

        // GET: TaskMains/Delete/5
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

        // POST: TaskMains/Delete/5
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
