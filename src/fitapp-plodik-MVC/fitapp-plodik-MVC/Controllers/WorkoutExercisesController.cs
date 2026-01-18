using fitapp_plodik_MVC.Data;
using fitapp_plodik_MVC.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace fitapp_plodik_MVC.Controllers
{
    public class WorkoutExercisesController : Controller
    {
        private readonly AppDbContext _db;

        public WorkoutExercisesController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Create(int workoutId)
        {
            ViewBag.WorkoutId = workoutId;
            ViewBag.ExerciseId = new SelectList(_db.Exercises.OrderBy(e => e.Name), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkoutExercise model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.WorkoutId = model.WorkoutId;
                ViewBag.ExerciseId = new SelectList(_db.Exercises.OrderBy(e => e.Name), "Id", "Name", model.ExerciseId);
                return View(model);
            }

            _db.WorkoutExercises.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Workouts", new { id = model.WorkoutId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.WorkoutExercises
                .Include(x => x.Exercise)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return NotFound();

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.WorkoutExercises.FindAsync(id);
            if (item == null)
                return NotFound();

            int workoutId = item.WorkoutId;

            _db.WorkoutExercises.Remove(item);
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Workouts", new { id = workoutId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.WorkoutExercises
                .Include(x => x.Exercise)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return NotFound();

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WorkoutExercise model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var item = await _db.WorkoutExercises.FindAsync(id);
            if (item == null)
                return NotFound();

            item.Sets = model.Sets;
            item.Reps = model.Reps;
            item.Weight = model.Weight;
            item.Note = model.Note;

            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Workouts", new { id = item.WorkoutId });
        }





























    }






}
