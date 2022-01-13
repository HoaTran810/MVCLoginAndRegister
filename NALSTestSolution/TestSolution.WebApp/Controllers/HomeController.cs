using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestSolution.Data.Constants;

namespace TestSolution.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            var fullName = HttpContext.Session.GetString("FullName");
            ViewData["FullName"] = fullName;

            return View();
        }
    }
}
