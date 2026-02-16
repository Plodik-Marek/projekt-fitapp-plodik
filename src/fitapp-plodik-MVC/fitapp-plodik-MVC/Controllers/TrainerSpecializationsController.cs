using fitapp_plodik_MVC.Data;
using fitapp_plodik_MVC.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace fitapp_plodik_MVC.Controllers
{
    public class TrainerSpecializationsController : Controller
    {
        private readonly AppDbContext _db;

        public TrainerSpecializationsController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Create(int trainerId)
        {
            ViewBag.TrainerId = trainerId;
            ViewBag.ExerciseId = new SelectList(_db.Exercises.OrderBy(e => e.Name), "Id", "Name");
            return View(new TrainerSpecialization { TrainerId = trainerId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainerSpecialization model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TrainerId = model.TrainerId;
                ViewBag.ExerciseId = new SelectList(_db.Exercises.OrderBy(e => e.Name), "Id", "Name", model.ExerciseId);
                return View(model);
            }

            bool exists = await _db.TrainerSpecializations.AnyAsync(x =>
                x.TrainerId == model.TrainerId && x.ExerciseId == model.ExerciseId);

            if (exists)
            {
                ModelState.AddModelError("", "Tato specializace už existuje."); 
                ViewBag.TrainerId = model.TrainerId;  // ViewBag umožnuje posílat data z controleru do view
                ViewBag.ExerciseId = new SelectList(_db.Exercises.OrderBy(e => e.Name), "Id", "Name", model.ExerciseId);
                return View(model);
            }

            _db.TrainerSpecializations.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Trainers", new { id = model.TrainerId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.TrainerSpecializations
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
            var item = await _db.TrainerSpecializations.FindAsync(id);
            if (item == null)
                return NotFound();

            int trainerId = item.TrainerId;

            _db.TrainerSpecializations.Remove(item);
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Trainers", new { id = trainerId });
        }
    }
}
