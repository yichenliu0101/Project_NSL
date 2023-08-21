using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nsl_Core.Models.Dtos.Member.Login;
using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Infra.Repositories.EFRepositories;
using Nsl_Core.Models.Interfaces;

namespace Nsl_Core.Controllers.Front
{
    public class TeacherListController : Controller
    {
        private readonly NSL_DBContext _db;
        private IHttpContextAccessor _accessor;
        private readonly NSL_DBContext _context;

        public TeacherListController(NSL_DBContext db, IHttpContextAccessor accessor, NSL_DBContext context)
        {
            _db = db;
            _accessor = accessor;
            _context = context;
        }
        public IActionResult Index(int? teacherid)
        {
            return View();
        }


        public IActionResult Info(int? id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult _TeacherTutorPartial()
        {
            return PartialView();
        }

    }
}
