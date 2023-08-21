using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Nsl_Core.Models.Dtos.Member.Login;
using Nsl_Core.Models.Dtos.Member.Manager;
using Nsl_Core.Models.Dtos.Teacher.TeacherResume;
using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Infra;
using Nsl_Core.Models.Infra.Repositories.DapperRepositories;
using Nsl_Core.Models.Infra.Repositories.EFRepositories;
using Nsl_Core.Models.Interfaces;
using NSL_html.Models.Infra;
using System.Security.Cryptography;
using System.Text.Json;

namespace Nsl_Core.Controllers.Front
{
    public class TeacherController : Controller
    {
        private readonly NSL_DBContext _context;
        private readonly IWebHostEnvironment _host;
        private readonly LoginDto _user;
        private readonly IConfiguration _configuration;

        public TeacherController(NSL_DBContext context, IWebHostEnvironment host, IConfiguration configuration)
        {
            _context = context;
            _host = host;
            _configuration = configuration;
        }

        public IActionResult Resume()
        {
            var user = JsonSerializer.Deserialize<LoginDto?>(JwtHelpers.DecodeToken(HttpContext.Request.Cookies.Get<string>("Login")));
            var repo = new TeacherRepositoriesDapper(_configuration);
            var model = repo.GetResume(user.Id);

            return View(model);
        }

        public IActionResult ResumeEditor()
        {
            var user = JsonSerializer.Deserialize<LoginDto?>(JwtHelpers.DecodeToken(HttpContext.Request.Cookies.Get<string>("Login")));
            var repo = new TeacherRepositoriesDapper(_configuration);
            var model = repo.GetResumeEdit(user.Id);

            return View(model);
        }


        [HttpPost]//接受view傳過來會員資料進行修改
        public IActionResult ResumeEditor(TeacherEditDto model)
        {
            //更新會員資料
            //s1 根據會員編號帶出要修改的資料
            var user = JsonSerializer.Deserialize<LoginDto?>(JwtHelpers.DecodeToken(HttpContext.Request.Cookies.Get<string>("Login")));
            var repo = new TeacherRepositoriesDapper(_configuration);
            var repo2 = new TeacherRepositoryEF(_context);
            try
            {
                var s1 = repo.GetResumeEdit(user.Id);
                //model會收到使用者修改的會員資料
                s1.BankCodeId = model.BankCodeId;
                s1.BankCodeName = model.BankCodeName;
                s1.BankAccount = model.BankAccount;
                s1.Education = model.Education;
                s1.WorkExperience = model.WorkExperience;
                s1.Title = model.Title;
                s1.Introduction = model.Introduction;
                //用model的資料更新到s1 讀取出來的資料

                int? bankiInDb = _context.TeachersResume.Where(x => x.BankAccount == model.BankAccount).Select(x => x.TeacherId).FirstOrDefault();
                if (bankiInDb == model.teacherId || bankiInDb == 0)
                {
                    var s2 = repo2.UpdateResume(s1);


                    TempData["EditTeacherResume"] = "履歷更新成功";
                }

                else
                {
                    
                    TempData["ErrMessage"] = "銀行帳號已存在，無法更新";
                }

                model = repo.GetResumeEdit(user.Id);
            }
            catch (Exception ex)
            {
                model = repo.GetResumeEdit(user.Id);
                TempData["ErrMessage"] = "履歷內容有誤，請重新編輯";
            }

            return View(model);
        }

        public IActionResult TutorInfo()
        {
            var user = JsonSerializer.Deserialize<LoginDto?>(JwtHelpers.DecodeToken(HttpContext.Request.Cookies.Get<string>("Login")));

            if (user == null) return RedirectToAction("Login", "NSL");

            var repo = new TeacherRepositoriesDapper(_configuration);

            var model = repo.GetTeacherTutorRecord(user.Id);
            return View(model);
        }

        public IActionResult TutorInfoEditor()
        {
            return View();
        }

        public IActionResult StudentInfo()
        {
            var user = JsonSerializer.Deserialize<LoginDto?>(JwtHelpers.DecodeToken(HttpContext.Request.Cookies.Get<string>("Login")));
            var repo = new TeacherRepositoriesDapper(_configuration);
            var model = repo.GetResume(user.Id);

            return View(model);
        }

        public IActionResult RevenueRecord()
        {
            return View();
        }

        public IActionResult _TeacherTutorPeriodPartial()
        {
            return PartialView();
        }

        public IActionResult _TeacherTutorRecordPartial()
        {
            return PartialView();
        }
    }
}
