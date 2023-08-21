using Microsoft.AspNetCore.Mvc;

namespace NSL_html.Controllers.Back
{
    public class BackHomeController : Controller
    {
        public IActionResult Index()
        {
            return PartialView();//不顯示Layout
        }

        public IActionResult Login()
        {
            return PartialView();
        }
		public IActionResult Signout()
		{
			HttpContext.Response.Cookies.Delete("Login");
			TempData.Remove("Login");
			return RedirectToAction("Index","NSL");
		}
	}
}
