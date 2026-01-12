using fitapp_plodik_MVC.Data;
using Microsoft.AspNetCore.Mvc;

namespace fitapp_plodik_MVC.Controllers
{
    public class TestController : Controller
    {
        
        private readonly AppDbContext _db;

        public TestController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var machines = _db.Machines.ToList();
            return Json(machines);
        }


    }
}
