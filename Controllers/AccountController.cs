using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Entity;
using MyApp.Models;
using System.Threading.Tasks;

namespace MyApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new users
                {
                    userName = model.userName,
                    userLastName = model.userLastName,
                    email = model.email,
                    password = model.password,
                    gender = model.gender
                };

                _context.users.Add(user);
                await _context.SaveChangesAsync();

                
                HttpContext.Session.SetString("UserEmail", user.email);
                HttpContext.Session.SetInt32("UserId", user.userId);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.users
                    .FirstOrDefaultAsync(u => u.email == model.userName && u.password == model.password);

                if (user != null)
                {
                    // Create session
                    HttpContext.Session.SetString("UserEmail", user.email);
                    HttpContext.Session.SetInt32("UserId", user.userId);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid email or password.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear session
            return RedirectToAction("Index", "Home"); // Redirect
        }
    }
}
