using fitapp_plodik_MVC.Data;
using fitapp_plodik_MVC.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace fitapp_plodik_MVC.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly AppDbContext _db;

        public ExercisesController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _db.Exercises
                .Include(e => e.Machine)
                .ToListAsync();

            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var exercise = await _db.Exercises
                .Include(e => e.Machine)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercise == null)
                return NotFound();

            return View(exercise);
        }

        public IActionResult Create()
        {
            ViewBag.MachineId = new SelectList(_db.Machines, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exercise exercise)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MachineId = new SelectList(_db.Machines, "Id", "Name", exercise.MachineId);
                return View(exercise);
            }

            _db.Exercises.Add(exercise);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var exercise = await _db.Exercises.FindAsync(id);
            if (exercise == null)
                return NotFound();

            ViewBag.MachineId = new SelectList(_db.Machines, "Id", "Name", exercise.MachineId);
            return View(exercise);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Exercise exercise)
        {
            if (id != exercise.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.MachineId = new SelectList(_db.Machines, "Id", "Name", exercise.MachineId);
                return View(exercise);
            }

            _db.Exercises.Update(exercise);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var exercise = await _db.Exercises
                .Include(e => e.Machine)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercise == null)
                return NotFound();

            return View(exercise);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercise = await _db.Exercises.FindAsync(id);
            if (exercise == null)
                return NotFound();

            _db.Exercises.Remove(exercise);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
