using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class TratamientoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
