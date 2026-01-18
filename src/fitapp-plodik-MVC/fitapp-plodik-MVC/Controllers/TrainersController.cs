using fitapp_plodik_MVC.Data;
using fitapp_plodik_MVC.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fitapp_plodik_MVC.Controllers
{
    public class TrainersController : Controller
    {
        private readonly AppDbContext _db;

        public TrainersController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            
            var data = await _db.Trainers
                .OrderBy(t => t.FullName)
                .ToListAsync();

            return View("Index",data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var trainer = await _db.Trainers
                .Include(t => t.TrainerSpecializations)
                    .ThenInclude(ts => ts.Exercise)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trainer == null)
                return NotFound();

            return View(trainer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trainer trainer)
        {
            if (!ModelState.IsValid)
                return View(trainer);

            _db.Trainers.Add(trainer);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var trainer = await _db.Trainers.FindAsync(id);
            if (trainer == null)
                return NotFound();

            return View(trainer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Trainer trainer)
        {
            if (id != trainer.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(trainer);

            _db.Trainers.Update(trainer);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await _db.Trainers.FirstOrDefaultAsync(t => t.Id == id);
            if (trainer == null)
                return NotFound();

            return View(trainer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainer = await _db.Trainers.FindAsync(id);
            if (trainer == null)
                return NotFound();

            _db.Trainers.Remove(trainer);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
