using fitapp_plodik_MVC.Data;
using fitapp_plodik_MVC.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fitapp_plodik_MVC.Controllers
{
    public class MachinesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public MachinesController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _db.Machines.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var machine = await _db.Machines.FindAsync(id);
            if (machine == null) return NotFound();
            return View(machine);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Machine machine, IFormFile? imageFile)
        {
            if (!ModelState.IsValid) return View(machine);

            if (imageFile != null && imageFile.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "img/uploads/machines");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(stream);

                machine.ImageUrl = "/img/uploads/machines/" + fileName;
            }

            _db.Machines.Add(machine);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var machine = await _db.Machines.FindAsync(id);
            if (machine == null) return NotFound();
            return View(machine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Machine machine, IFormFile? imageFile)
        {
            if (id != machine.Id) return NotFound();

            var existingMachine = await _db.Machines
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (existingMachine == null) return NotFound();

            if (!ModelState.IsValid) return View(machine);

            if (imageFile != null && imageFile.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "img/uploads/machines");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(stream);

                machine.ImageUrl = "/img/uploads/machines/" + fileName;
            }
            else
            {
                machine.ImageUrl = existingMachine.ImageUrl;
            }

            _db.Update(machine);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var machine = await _db.Machines.FindAsync(id);
            if (machine == null) return NotFound();
            return View(machine);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var machine = await _db.Machines.FindAsync(id);
            if (machine == null) return NotFound();

            _db.Machines.Remove(machine);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
