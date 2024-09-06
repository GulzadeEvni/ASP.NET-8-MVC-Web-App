using Microsoft.AspNetCore.Mvc;

namespace MyApp.Controllers
{
    public class AboutController : Controller
    {

        public IActionResult About() {
        
        
            return View();
        }
    }
}
