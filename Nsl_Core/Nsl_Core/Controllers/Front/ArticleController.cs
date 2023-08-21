using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nsl_Core.Models;
using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Infra;

namespace Nsl_Core.Controllers.Front
{
	public class ArticleController : Controller
    {
        private NSL_DBContext _db;
        public ArticleController(NSL_DBContext db)
        {
            _db = db;
        }
		public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            var article = _db.Articles.Find(id);

            return View(article);
        }
    }
}
