using fitapp_plodik_MVC.Data;
using fitapp_plodik_MVC.Entities;
using fitapp_plodik_MVC.Security;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace fitapp_plodik_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /*
        public IActionResult TestHash()
        {
            var hash = PasswordHelper.HashPassword("marek");   // metoda pro získání hashovaného hesla 
            return Content(hash);
        }
        */



        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                ViewBag.Error = "Uživatel neexistuje";
                return View();
            }

            bool isValid = PasswordHelper.Verify(password, user.Password);

            if (!isValid)
            {
                ViewBag.Error = "Špatné heslo";
                return View();
            }

            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserEmail", user.Email);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Register(string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Hesla se neshodují";
                return View();
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser != null)
            {
                ViewBag.Error = "Uživatel s tímto emailem už existuje";
                return View();
            }

            var newUser = new User
            {
                Email = email,
                Password = PasswordHelper.HashPassword(password)
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }





        [HttpGet]
        public IActionResult Profile()
        {
            var userIdString = HttpContext.Session.GetString("UserId");

            if (userIdString == null)
                return RedirectToAction("Login");

            int userId = int.Parse(userIdString);

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }




        [HttpPost]
        public IActionResult Profile(User model)
        {
            var userIdString = HttpContext.Session.GetString("UserId");

            if (userIdString == null)
                return RedirectToAction("Login");

            int userId = int.Parse(userIdString);

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
                return RedirectToAction("Login");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Age = model.Age;

            _context.SaveChanges();

            ViewBag.Success = "Profil byl úspěšně uložen.";
            return View(user);
        }




        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Session");

            return RedirectToAction("Login");
        }
    }
}
