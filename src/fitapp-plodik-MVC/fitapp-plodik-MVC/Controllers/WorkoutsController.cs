using fitapp_plodik_MVC.Data;
using fitapp_plodik_MVC.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fitapp_plodik_MVC.Controllers
{
    public class WorkoutsController : Controller
    {
        private readonly AppDbContext _db;

        public WorkoutsController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _db.Workouts
                .OrderByDescending(w => w.WorkoutDate)
                .ToListAsync();

            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var workout = await _db.Workouts
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (workout == null)
                return NotFound();

            return View(workout);
        }


        public IActionResult Create()
        {
            var model = new Workout
            {
                WorkoutDate = DateTime.Today
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Workout workout)
        {
            if (!ModelState.IsValid)
                return View(workout);

            _db.Workouts.Add(workout);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var workout = await _db.Workouts.FindAsync(id);
            if (workout == null)
                return NotFound();

            return View(workout);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Workout workout)
        {
            if (id != workout.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(workout);

            _db.Workouts.Update(workout);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var workout = await _db.Workouts.FirstOrDefaultAsync(w => w.Id == id);
            if (workout == null)
                return NotFound();

            return View(workout);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workout = await _db.Workouts.FindAsync(id);
            if (workout == null)
                return NotFound();

            _db.Workouts.Remove(workout);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
