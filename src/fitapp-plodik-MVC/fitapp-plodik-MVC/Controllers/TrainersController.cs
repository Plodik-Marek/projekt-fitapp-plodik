using fitapp_plodik_MVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using fitapp_plodik_MVC.Entities;

public class MachinesController : Controller
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public MachinesController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    public async Task<IActionResult> Index() => View(await _db.Machines.ToListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var machine = await _db.Machines.FindAsync(id);
        if (machine == null) return NotFound();
        return View(machine);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Machine machine, IFormFile? imageFile)
    {
        if (!ModelState.IsValid) return View(machine);

        if (imageFile != null)
        {
            string folder = Path.Combine(_env.WebRootPath, "img/uploads/machines");
            Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            using var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create);
            await imageFile.CopyToAsync(stream);

            machine.ImageUrl = "/img/uploads/machines/" + fileName;
        }

        _db.Add(machine);
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

        if (imageFile != null)
        {
            string folder = Path.Combine(_env.WebRootPath, "img/uploads/machines");
            Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            using var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create);
            await imageFile.CopyToAsync(stream);

            machine.ImageUrl = "/img/uploads/machines/" + fileName;
        }

        _db.Update(machine);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
