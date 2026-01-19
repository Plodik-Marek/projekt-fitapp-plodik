using fitapp_plodik_MVC.Data;
using fitapp_plodik_MVC.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;

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
        public async Task<IActionResult> Create(Exercise exercise, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MachineId = new SelectList(_db.Machines, "Id", "Name", exercise.MachineId);
                return View(exercise);
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/icons/exercises");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                exercise.ImageUrl = "/img/icons/exercises/" + fileName;
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
        public async Task<IActionResult> Edit(int id, Exercise exercise, IFormFile? imageFile)
        {
            if (id != exercise.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.MachineId = new SelectList(_db.Machines, "Id", "Name", exercise.MachineId);
                return View(exercise);
            }

            var dbExercise = await _db.Exercises.FirstOrDefaultAsync(e => e.Id == id);
            if (dbExercise == null)
                return NotFound();

            dbExercise.Name = exercise.Name;
            dbExercise.MuscleGroup = exercise.MuscleGroup;
            dbExercise.Description = exercise.Description;
            dbExercise.MachineId = exercise.MachineId;

            if (imageFile != null && imageFile.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(dbExercise.ImageUrl) && dbExercise.ImageUrl.StartsWith("/img/"))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", dbExercise.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/icons/exercises");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                dbExercise.ImageUrl = "/img/icons/exercises/" + fileName;
            }

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

            if (!string.IsNullOrWhiteSpace(exercise.ImageUrl) && exercise.ImageUrl.StartsWith("/img/"))
            {
                var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", exercise.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (System.IO.File.Exists(imgPath))
                    System.IO.File.Delete(imgPath);
            }

            _db.Exercises.Remove(exercise);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
