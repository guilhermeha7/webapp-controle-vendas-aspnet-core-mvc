using Microsoft.AspNetCore.Mvc;

namespace SalesWebMvc.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
