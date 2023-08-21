using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Nsl_Core.Models.Dtos.Member.Login;
using Nsl_Core.Models.Dtos.Member.Manager;
using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Infra;
using Nsl_Core.Models.Infra.RandomGenerate;
using Nsl_Core.Models.Infra.Repositories.DapperRepositories;
using Nsl_Core.Models.Infra.Repositories.EFRepositories;
using Nsl_Core.Models.Interfaces;
using Nsl_Core.Models.Services;
using NSL_html.Models.Infra;
using System.Diagnostics.Metrics;
using System.Text.Json;
using XAct.Library.Settings;

namespace Nsl_Core.Controllers.Front
{
    public class MemberController : Controller
    {
        private readonly NSL_DBContext _context;
        private readonly IWebHostEnvironment _host;
        private readonly IConfiguration _configuration;


        public MemberController(NSL_DBContext context, IWebHostEnvironment host, IConfiguration configuration)
        {
            _context = context;
            _host = host;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }

        //根據會員id讀取資料 傳到view進行修改
        [HttpGet]
        public IActionResult BasicInfoManage()
        {
            //顯示會員資料

            var user = JsonSerializer.Deserialize<LoginDto?>(JwtHelpers.DecodeToken(HttpContext.Request.Cookies.Get<string>("Login")));
            IMemberRepo repo = new MemberRepositoryEF(_context);
            var model = repo.Get(user.Id);

            return View(model);
        }

        [HttpPost]//接受view傳過來會員資料進行修改
        public IActionResult BasicInfoManage(MemberDto model, IFormFile ImageName)
        {
            var user = JsonSerializer.Deserialize<LoginDto?>(JwtHelpers.DecodeToken(HttpContext.Request.Cookies.Get<string>("Login")));

            //更新會員資料
            //s1 根據會員編號帶出要修改的資料
            IMemberRepo repo = new MemberRepositoryEF(_context);
            var memberfile = repo.Get(user.Id);
            


            if (model.ConfirmPassword != model.Password)
            {
                TempData["ErrMessage"] = "密碼錯誤!無法更新";
                model.ImageName = memberfile.ImageName;


                return View(memberfile);
            }
            //上傳圖片檔案名稱
            try
            {

                if (ImageName != null)
                {
                    string photoName = string.Empty;
                    photoName = ImageName.HashImageName();
                    // model.ImageName = photoName;
                    memberfile.ImageName = photoName;
                    string path = Path.Combine(_host.WebRootPath, "uploads", photoName);
                    using (var filestream = new FileStream(path, FileMode.Create))
                    {
                        ImageName.CopyTo(filestream);
                    }
                }
                //else
                //{
                //    memberfile.ImageName = "default.jpg";
                //}

                //model會收到使用者修改的會員資料

                memberfile.Password = model.Password;
                memberfile.Name = model.Name;
                memberfile.AreaId = model.AreaId;
                memberfile.CityId = model.CityId;
                memberfile.Email = model.Email;
                memberfile.Phone = model.Phone;
                memberfile.Birthday = model.Birthday;
                memberfile.Gender = model.Gender;
                memberfile.ConfirmPassword = model.ConfirmPassword;

                //用model的資料更新到memberfile讀取出來的資料
                repo.Update(memberfile);
                _context.SaveChanges();

                model = repo.Get(model.Id);
                user.Name = model.Name;
                user.ImageName = memberfile.ImageName;
                HttpContext.Response.Cookies.Set("Login", new JwtHelpers(_configuration).GenerateToken(user));

                TempData["EditTeacherResume"] = "個人資料更新成功";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(model);

        }





        public IActionResult CoursesManage()
        {
            return View();
        }

        public IActionResult Calendar()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ConsumptionRecord(int pageIndex)
        {
            var user = JsonSerializer.Deserialize<LoginDto?>(JwtHelpers.DecodeToken(HttpContext.Request.Cookies.Get<string>("Login")));
            IMemberRepo repo = new MemberRepositoryEF(_context);

            var model = repo.GetList(user.Id);
            ViewBag.TotalPage = (int)Math.Ceiling(model.Count()/5.0);
            var pagingModel = model.PageList(5, pageIndex);
            return View(pagingModel);
        }

        [HttpGet]
        public IActionResult TutorRecord(int pageIndex)
        {
            var user = JsonSerializer.Deserialize<LoginDto?>(JwtHelpers.DecodeToken(HttpContext.Request.Cookies.Get<string>("Login")));
            var repo = new MemberRepo(_configuration);
            List<MemberTutorRecordDto> model = repo.GetTutorRecords(user.Id);
            ViewBag.TotalPage = (int)Math.Ceiling(model.Count() / 5.0);
            var pagingModel = model.PageList(5, pageIndex);
            return View(pagingModel);
        }

        [HttpPost]
        public IActionResult CommentEdit(int memberTutorPeriodId, string commentContent, double satisfaction, int id)
        {
            if (!string.IsNullOrEmpty(commentContent))
            {
                var repo = new MemberCommentEFRepo(_context);
                var result = repo.EditOrCreate(memberTutorPeriodId, commentContent, satisfaction);

                return Content(result);
            }
            return Content("請重新輸入");
        }

        public IActionResult MyCoupons()
        {
            return View();
        }

        public IActionResult ReserveTutorPeriod()
        {
            return View();
        }

        public IActionResult _TutorPeriodPartial()
        {
            return PartialView();
        }

        public IActionResult _MemberPeriodPartial()
        {
            return PartialView();
        }

        public IActionResult _MemberFileCreate()
        {
            var randomGenerate = new RandomEntity(_context, _configuration);
            randomGenerate.CreateMember();
            return View();
        }

        public IActionResult RandomGenerateData()
        {
            string msg = "success";
            try
            {
                var random = new RandomGenerator(_context, _configuration);
                //random.RandomTutorPeriod();
                //random.RandomComment();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return BadRequest(msg);
        }
    }
}

