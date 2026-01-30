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
        private readonly IWebHostEnvironment _env;

        public ExercisesController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _db.Exercises.Include(e => e.Machine).ToListAsync();
            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var exercise = await _db.Exercises
                .Include(e => e.Machine)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercise == null) return NotFound();
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
                string folder = Path.Combine(_env.WebRootPath, "img/uploads/exercises");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(stream);

                exercise.ImageUrl = "/img/uploads/exercises/" + fileName;
            }

            _db.Add(exercise);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var exercise = await _db.Exercises.FindAsync(id);
            if (exercise == null) return NotFound();

            ViewBag.MachineId = new SelectList(_db.Machines, "Id", "Name", exercise.MachineId);
            return View(exercise);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Exercise exercise, IFormFile? imageFile)
        {
            if (id != exercise.Id) return NotFound();

            var existing = await _db.Exercises.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.MachineId = new SelectList(_db.Machines, "Id", "Name", exercise.MachineId);
                return View(exercise);
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "img/uploads/exercises");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(stream);

                exercise.ImageUrl = "/img/uploads/exercises/" + fileName;
            }
            else
            {
                exercise.ImageUrl = existing.ImageUrl;
            }

            _db.Update(exercise);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var exercise = await _db.Exercises
                .Include(e => e.Machine)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercise == null) return NotFound();
            return View(exercise);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercise = await _db.Exercises.FindAsync(id);
            if (exercise == null) return NotFound();

            _db.Exercises.Remove(exercise);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
