using Microsoft.AspNetCore.Mvc;
using Nsl_Core.Models.ViewModels;
using NuGet.Packaging;
using NSL_html.Models.Infra;
using Nsl_Core.Models.Infra.Repositories.EFRepositories;
using Nsl_Core.Models.Interfaces;
using Nsl_Core.Models.Services;
using Nsl_Core.Models.EFModels;

namespace NSL_html.Controllers.Back
{
    public class BackMemberController : Controller
    {
		private readonly NSL_DBContext _db;
		private readonly IWebHostEnvironment _host;

		public BackMemberController(NSL_DBContext db, IWebHostEnvironment host)
		{
			_db = db;
			_host = host;	

		}
		public IActionResult Index()
        {
            return View();
        }

		public IActionResult CAU(int? id)
		{
			IMemberRepo repo = new MemberRepositoryEF(_db);
			var dto = repo.Get(id);

			return View(dto);
		}

		public IActionResult CreateAdmin()
        {
            return View();
        }

		[HttpPost]
		public IActionResult CreateAdmin(AdminVM admin, IFormFile photo)
		{
			string photoName = string.Empty;
			if (photo != null)
			{
				photoName = photo.HashImageName();
				
				admin.ImageName = photoName;
			}

			var service = new AdminService(_db);
			try
			{
				int newId = service.Create(admin);
				if (photo != null) photo.UploadResult(_host, photoName);
				return RedirectToAction("ArticleIndex", "BackMember");
			}
			catch (Exception ex)
			{
				return RedirectToAction("CreateAdmin", "BackMember");
			}
		}
	}
}
