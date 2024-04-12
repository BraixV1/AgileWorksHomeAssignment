using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Controllers
{
    public class ToDoTaskController : Controller
    {
        private readonly AppDbContext _context;

        public ToDoTaskController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ToDoTask
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ToDoTasks.ToListAsync());
        }
        
        // POST: ToDoTask
        [HttpPost]
        public async Task<IActionResult> Index(Guid? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var toDoTask = await _context.ToDoTasks.FirstOrDefaultAsync(m => m.Id == id);
            if (toDoTask == null)
            {
                return NotFound();
            }
            
            toDoTask.CompletedAtDt = DateTime.Now;
            _context.ToDoTasks.Update(toDoTask);
            await _context.SaveChangesAsync();
            return View(await _context.ToDoTasks.ToListAsync());
        }

        // GET: ToDoTask/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoTask = await _context.ToDoTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoTask == null)
            {
                return NotFound();
            }

            return View(toDoTask);
        }

        // GET: ToDoTask/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDoTask/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,HasToBeDoneAtDt,Id")] ToDoTask toDoTask)
        {
            toDoTask.CreatedAtDt = DateTime.Now;
            if (ModelState.IsValid)
            {
                toDoTask.Id = Guid.NewGuid();
                _context.Add(toDoTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toDoTask);
        }

        // GET: ToDoTask/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoTask = await _context.ToDoTasks.FindAsync(id);
            if (toDoTask == null)
            {
                return NotFound();
            }
            return View(toDoTask);
        }

        // POST: ToDoTask/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Description,CreatedAtDt,HasToBeDoneAtDt,CompletedAtDt,Id")] ToDoTask toDoTask)
        {
            if (id != toDoTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDoTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoTaskExists(toDoTask.Id))
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
            return View(toDoTask);
        }

        // GET: ToDoTask/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoTask = await _context.ToDoTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoTask == null)
            {
                return NotFound();
            }

            return View(toDoTask);
        }

        // POST: ToDoTask/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var toDoTask = await _context.ToDoTasks.FindAsync(id);
            if (toDoTask != null)
            {
                _context.ToDoTasks.Remove(toDoTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoTaskExists(Guid id)
        {
            return _context.ToDoTasks.Any(e => e.Id == id);
        }
    }
}
