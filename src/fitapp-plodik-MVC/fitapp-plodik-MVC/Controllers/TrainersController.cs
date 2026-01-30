using fitapp_plodik_MVC.Data;
using fitapp_plodik_MVC.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fitapp_plodik_MVC.Controllers
{
    public class TrainersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public TrainersController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _db.Trainers.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var trainer = await _db.Trainers.FindAsync(id);
            if (trainer == null) return NotFound();
            return View(trainer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trainer trainer, IFormFile? imageFile)
        {
            if (!ModelState.IsValid) return View(trainer);

            if (imageFile != null && imageFile.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "img/uploads/trainers");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(stream);

                trainer.ImageUrl = "/img/uploads/trainers/" + fileName;
            }

            _db.Trainers.Add(trainer);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var trainer = await _db.Trainers.FindAsync(id);
            if (trainer == null) return NotFound();
            return View(trainer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Trainer trainer, IFormFile? imageFile)
        {
            if (id != trainer.Id) return NotFound();

            var existingTrainer = await _db.Trainers
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existingTrainer == null) return NotFound();

            if (!ModelState.IsValid) return View(trainer);

            if (imageFile != null && imageFile.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "img/uploads/trainers");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(stream);

                trainer.ImageUrl = "/img/uploads/trainers/" + fileName;
            }
            else
            {
                trainer.ImageUrl = existingTrainer.ImageUrl;
            }

            _db.Update(trainer);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await _db.Trainers.FindAsync(id);
            if (trainer == null) return NotFound();
            return View(trainer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainer = await _db.Trainers.FindAsync(id);
            if (trainer == null) return NotFound();

            _db.Trainers.Remove(trainer);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
