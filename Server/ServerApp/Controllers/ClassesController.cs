using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Controllers
{
    [Authorize]
    public class ClassesController : Controller
    {
        private readonly AppDbContext _context;

        public ClassesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Classes
        public async Task<IActionResult> Index()
        {
            var classes = _context.Classes.Include(t => t.Coach);
            return View(await classes.ToListAsync());
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingClass = await _context.Classes
                .Include(t => t.Coach)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (trainingClass == null)
            {
                return NotFound();
            }

            return View(trainingClass);
        }

        // GET: Classes/Create
        public IActionResult Create()
        {
            ViewData["CoachID"] = new SelectList(_context.Coaches, "ID", "Name");
            return View();
        }

        // POST: Classes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,CoachID,TimeSlot")] TrainingClass trainingClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainingClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CoachID"] = new SelectList(_context.Coaches, "ID", "Name", trainingClass.CoachID);
            return View(trainingClass);
        }

        // GET: Classes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingClass = await _context.Classes.FindAsync(id);
            if (trainingClass == null)
            {
                return NotFound();
            }
            ViewData["CoachID"] = new SelectList(_context.Coaches, "ID", "Name", trainingClass.CoachID);
            return View(trainingClass);
        }

        // POST: Classes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,CoachID,TimeSlot")] TrainingClass trainingClass)
        {
            if (id != trainingClass.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingClassExists(trainingClass.ID))
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
            ViewData["CoachID"] = new SelectList(_context.Coaches, "ID", "Name", trainingClass.CoachID);
            return View(trainingClass);
        }

        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingClass = await _context.Classes
                .Include(t => t.Coach)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (trainingClass == null)
            {
                return NotFound();
            }

            return View(trainingClass);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingClass = await _context.Classes.FindAsync(id);
            if (trainingClass != null)
            {
                _context.Classes.Remove(trainingClass);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingClassExists(int id)
        {
            return _context.Classes.Any(e => e.ID == id);
        }
    }
}
