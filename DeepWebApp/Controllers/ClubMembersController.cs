using Microsoft.AspNetCore.Mvc;

namespace DeepWebApp.Controllers
{
    public class ClubMembersController : Controller
    {
        public IActionResult Index(string viewName, int id)
        {
            return View(viewName);
        }
    }
}
