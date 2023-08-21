using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Nsl_Core.Models;
using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Infra.LoginAPI;
using System.Diagnostics;

namespace Nsl_Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
