using Microsoft.AspNetCore.Mvc;

namespace Carrito_D.Controllers
{
    public class General : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
