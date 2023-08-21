using Microsoft.AspNetCore.Mvc;
using Nsl_Core.Models;
using Nsl_Core.Models.Infra.Repositories.DapperRepositories;
using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Infra.Repositories.EFRepositories;
using Nsl_Core.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Nsl_Core.Models.Dtos;
using NSL_html.Models.Infra;
using Nsl_Core.Models.Infra;
using Nsl_Core.Models.Dtos.Teacher.TeacherResume;

namespace NSL_html.Controllers.Back
{
    public class BackTeacherController : Controller
    {
        private readonly NSL_DBContext _db;
        private readonly IWebHostEnvironment _host;
        private readonly IConfiguration _configuration;
        public BackTeacherController(NSL_DBContext db, IWebHostEnvironment host, IConfiguration configuration)
        {
            _db = db;
            _host = host;
            _configuration = configuration;
        }


        [HttpPost]//接受view傳過來會員資料進行修改
        public IActionResult Verify(int teacherId)
        {
            ITeacherRepo repo = new TeacherRepositoryEF(_db);
            try
            {
                var tMemberInfo = repo.Update(teacherId);
                EmailHelper.VerifyTeacher(tMemberInfo);
                repo.CreateResume(teacherId);
			}
            catch(ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction("ApplyList");
 
        }
        public IActionResult ApplyList()
        {
            return View();
		}

        public IActionResult CAU(int id)
        {
            var repo = new TeacherRepositoriesDapper(_configuration);
            var dto=repo.GetBackTeacherResume(id);

            return View(dto);
        }

        public IActionResult Application(int id)
        {
            var repo = new TeacherRepositoriesDapper(_configuration);
            var dto = repo.GetApply(id);
            return View(dto);
        }

        public IActionResult Comment(int id)
        {
            var repo = new TeacherRepositoriesDapper(_configuration);
            var dto = repo.GetBackTeacherResume(id);

            return View(dto);
        }

        //public IActionResult GetComment(int id)
        //{
        //    var repo = new TeacherRecordRepo(_configuration);
        //    var dto = repo.GetComments(id);
        //    return Json(dto);
        //}

		public IActionResult ResumeList()
		{
            return View();
		}
	}
}
