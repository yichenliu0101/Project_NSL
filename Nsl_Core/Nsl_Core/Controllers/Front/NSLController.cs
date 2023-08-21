using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Nsl_Core.Models.Dtos;
using Nsl_Core.Models.Dtos.Website.Index;
using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Infra;
using Nsl_Core.Models.Infra.EntitiesTransfer;
using Nsl_Core.Models.Infra.Repositories.EFRepositories;
using Nsl_Core.Models.ViewModels;
using System.Reflection;
using System.Text.Json;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Runtime.ConstrainedExecution;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using XAct.Users;
using System.IdentityModel.Tokens.Jwt;
using Google.Apis.Auth;
using Nsl_Core.Models.Infra.LoginAPI;
using Nsl_Core.Models.Dtos.Member.Login;
using Humanizer;
using Facebook;
using Microsoft.AspNetCore.Authentication.Facebook;
using System.IO;
using Microsoft.Extensions.Configuration;
using XAct.Library.Settings;
using DotNetOpenAuth.AspNet.Clients;
using FacebookClient = Facebook.FacebookClient;
using System.Web;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net;
using Nsl_Core.Models.Dtos.Member.Manager;
using Nsl_Core.Models.Interfaces;
using NSL_html.Models.Infra;
using System.Configuration;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.AspNetCore.Hosting;
using NuGet.Protocol.Plugins;
using Message = Nsl_Core.Models.EFModels.Message;

namespace Nsl_Core.Controllers.Front
{
    public class NSLController : Controller
    {

        private readonly NSL_DBContext _db;
        private readonly JwtHelpers _jwtHelpers;
        private readonly IConfiguration _configuration;
        private readonly NSLRepositoriresEF _repo;
        private readonly IWebHostEnvironment _host;

        public NSLController(NSL_DBContext db, JwtHelpers jwtHelpers, IConfiguration configuration, IWebHostEnvironment host)
        {
            _db = db;
            _jwtHelpers = jwtHelpers;
            _repo = new NSLRepositoriresEF(_db);
            _configuration = configuration;
            _host = host;
        }

        public IActionResult Index()
        {
            var repo = new NSLRepositoriresEF(_db);

            return View(repo.IndexDto);
        }
        //[TypeFilter(typeof(NotMemberVerify))]
        public IActionResult Login()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl( new
            {
                client_id = "your_facebook_client_id",
                redirect_uri = "https://localhost:7217/NSL/FacebookRedirect",
                scope = "email"
            });
            ViewBag.Url = loginUrl;

           
            return View();
        }

        public ActionResult FacebookRedirect(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Get("/oauth/access_token", new
            {
                client_id = "your_facebook_client_id",
                client_secret = "your_facebook_client_secret",
                redirect_uri = "https://localhost:7217/NSL/FacebookRedirect",
                code = code
            });
            fb.AccessToken = result.AccessToken;

            dynamic me = fb.Get("/me?fields=name,email");
            string name = me.name;
            string email = me.email;
            return RedirectToAction("Login");

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM vm)
        {
            var isMembers = new HashPassword(_db).VerifyPassword(vm.Password, vm.Account);

            if (isMembers == false)
            {
                TempData["ErrMessage"] = "登入失敗，請重新再試";
                return View();
            }
            else
            {
                var dto = _db.Members.Where(x => x.Email == vm.Account).FirstOrDefault().ToLoginDto();
				var token = _jwtHelpers.GenerateToken(dto);
				HttpContext.Response.Cookies.Set("Login", token);
				TempData["Login"] = dto.Name;
			}

            return RedirectToAction("Index");
        }


        public IActionResult Signout()
        {
            HttpContext.Response.Cookies.Delete("Login");
            TempData.Remove("Login");
            return RedirectToAction("Index");
        }
        //[TypeFilter(typeof(AdminVerify))]
        //[TypeFilter(typeof(TeacherVerify))]
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterVM vm)
        {

            if (!ModelState.IsValid) return View(vm);
            try
            {
                _repo.Register(vm);
                TempData["VerifyMember"] = "驗證信已寄出，請盡速登入您的電子信箱以完成會員驗證！";

                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["ErrMessage"] = ex.Message;

                return View();
            }
            
        }

        [HttpPost]
        public IActionResult ForgetPassword(ForgetPasswordVM vm)
        {

            if (!ModelState.IsValid) return View();
            try
            {
                _repo.ForgetPassword(vm);

                TempData["ForgetPassword"] = "驗證信已寄出，請盡速登入您的電子信箱以完成新密碼的設置！";

                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrMessage"] = ex.Message;

                return View();
            }

        }

        [HttpPost]//接受view傳過來會員資料進行修改
        public IActionResult VerifyPassword(VerifyPasswordVM vm,string token)
        {

            try
            {

                if (!ModelState.IsValid) return View(vm);
                if (vm.ConfirmPassword != vm.Password)
                {
                    TempData["ErrMessage"] = "密碼錯誤!無法更新";

                    return View();
                }

                var member = _db.Members.FirstOrDefault(u => u.EmailToken == token);
                if (member != null)
                {
                    string hashPassword = new HashPassword(_db).GenerateHashPassword(vm.Password, out string salt);

                    member.Password = vm.Password;
                    member.Salt = salt;
                    member.EncryptedPassword = hashPassword;
                    _db.SaveChanges();
                }

                TempData["EditTeacherResume"] = "新密碼設置成功";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return RedirectToAction("Login");

        }

        public IActionResult VerifyPassword()
        {
            return View();
        }
        public IActionResult Verify(string token)
        {

            if (!string.IsNullOrEmpty(token))
            {
                bool isValidToken = VerifyToken(token);

                if (isValidToken)
                {
                    MarkUserAsVerified(token);
                    ViewBag.VerificationStatus = "驗證成功！";
                }
                else
                {
                    ViewBag.VerificationStatus = "驗證失敗。";
                }
            }
            else
            {
                ViewBag.VerificationStatus = "驗證連結無效。";
            }

            return PartialView();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        private bool VerifyToken(string token)
        {
            var member = _db.Members.FirstOrDefault(u => u.EmailToken == token);
            return member != null;
        }

        private void MarkUserAsVerified(string token)
        {
            var member = _db.Members.FirstOrDefault(u => u.EmailToken == token);
            if (member != null)
            {
                member.EmailCheck= true;
                _db.SaveChanges();
            }
        }

        public IActionResult test()
        {

            var dto = new Message()
            {
                SenderId = 2,
                RecipientId = 1,
                MessageText = "2222",
                CreateTime = DateTime.Now,
            };

            _db.Message.Add(dto);
            _db.SaveChanges(true);
            //todo
            return Content("成功");
        }

        public IActionResult ValidGoogleLogin()
        {
            string? formCredential = Request.Form["credential"]; //回傳憑證
            string? formToken = Request.Form["g_csrf_token"]; //回傳令牌
            string? cookiesToken = Request.Cookies["g_csrf_token"]; //Cookie 令牌

            // 驗證 Google Token
            GoogleJsonWebSignature.Payload? payload = new GoogleLoginApi().VerifyGoogleToken(formCredential, formToken, cookiesToken).Result;

            //確認email有沒有在資料庫中
            var account = _db.Members.Where(o => o.Email == payload.Email).FirstOrDefault();
            LoginDto login = new();
            if (account == null) //沒有的話就註冊
            {
                var register = new RegisterVM() { Email= payload.Email,Name = payload.Name,ImageName = "default.jpg", Gender=true,};//實體化
                login = _repo.Register(register);
                
            }
            else
            {
                login = new LoginDto() {Id = account.Id, Name = payload.Name, ImageName = account.ImageName, Role = account.Role};
            }
            //最後再將傳回的LoginDto放置在Cookie中
            var token = _jwtHelpers.GenerateToken(login);
            HttpContext.Response.Cookies.Set("Login", token);
            //var xx=HttpContext.Request.Cookies.Get<LoginDto>("Login");
            TempData["Login"] = login.Name;

            string email = payload.Email;
            string name = payload.Name;
            string picture = payload.Picture;

            return RedirectToAction("Index");
        }

        public ActionResult LineLoginDirect()
        {
            string response_type = "code";
            string client_id = "your_client_id";
            string redirect_uri = HttpUtility.UrlEncode("https://localhost:7217/NSL/callback");
            string state = "aaa";
            string LineLoginUrl = string.Format("https://access.line.me/oauth2/v2.1/authorize?response_type={0}&client_id={1}&redirect_uri={2}&state={3}&scope=openid%20profile&nonce=09876xyz",
                response_type,
                client_id,
                redirect_uri,
                state
                );
            return Redirect(LineLoginUrl);
        }

        public async Task<ActionResult> callback(string code, string state)
        {
            if (state == "aaa")
            {
                #region Api變數宣告
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                string result = string.Empty;
                NameValueCollection nvc = new NameValueCollection();
                #endregion
                try
                {
                    //取回Token
                    string ApiUrl_Token = "https://api.line.me/oauth2/v2.1/token";
                    nvc.Add("grant_type", "authorization_code");
                    nvc.Add("code", code);
                    nvc.Add("redirect_uri", "https://localhost:7217/NSL/callback");
                    nvc.Add("client_id", "your_client_id");
                    nvc.Add("client_secret", "your_client_secret");
                    string JsonStr = Encoding.UTF8.GetString(wc.UploadValues(ApiUrl_Token, "POST", nvc));
                    LineLoginToken ToKenObj = JsonConvert.DeserializeObject<LineLoginToken>(JsonStr);
                    wc.Headers.Clear();

                    //取回User Profile
                    string ApiUrl_Profile = "https://api.line.me/v2/profile";
                    wc.Headers.Add("Authorization", "Bearer " + ToKenObj.access_token);
                    string UserProfile = wc.DownloadString(ApiUrl_Profile);
                    var userProfileArray = UserProfile.Split(",");
                    var userImage = userProfileArray[2].Split(":")[1] + userProfileArray[2].Split(":")[2];
                    userImage = userImage.Substring(1, userImage.Length - 3);
                    userImage = userImage.Insert(5, ":");

                    string imageFileName = await CopyImage(userImage);

                    LineProfile ProfileObj = JsonConvert.DeserializeObject<LineProfile>(UserProfile);

                    var account = _db.Members.Where(o => o.LineId == ProfileObj.userId).FirstOrDefault();
                    LineLoginDto login = new();
                    if (account == null) //沒有的話就註冊
                    {
                        //byte[] imageBytes = wc.DownloadData(ProfileObj.pictureUrl);
                        //string base64Image = Convert.ToBase64String(imageBytes);

                        var register = new LineRegisterVM() { LineId =ProfileObj.userId, Name = ProfileObj.displayName, ImageName= imageFileName, Emailcheck =true };//實體化
                        login = _repo.LineRegister(register);

                        var token2 = _jwtHelpers.GenerateToken(login);
                        HttpContext.Response.Cookies.Set("Login", token2);


                        return RedirectToAction("LineFastLogin");

                    }
                    else
                    {
                        login = new LineLoginDto() { Id = account.Id, Name = ProfileObj.displayName, ImageName = imageFileName, Role = account.Role, EmailCheck = (bool)account.EmailCheck };
                    }

                    var token = _jwtHelpers.GenerateToken(login);
                    HttpContext.Response.Cookies.Set("Login", token);
                    //var xx=HttpContext.Request.Cookies.Get<LoginDto>("Login");
                    TempData["Login"] = login.Name;


                    //最後再將傳回的LoginDto放置在Cookie中
                    string lineId = ProfileObj.userId;
                    string name = ProfileObj.displayName;
                    string picture = imageFileName;

                    //return RedirectToAction("UserProfile", "Home", new { displayName = ProfileObj.displayName, pictureUrl = ProfileObj.pictureUrl });
                    //return RedirectToAction("Index", "NSL", new { displayName = ProfileObj.displayName, pictureUrl = ProfileObj.pictureUrl, Email = ProfileObj.Email });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    throw;
                }
            }
            return View("Index");
        }
        public async Task<string> CopyImage(string imageUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    byte[] imageBytes = await client.GetByteArrayAsync(imageUrl);

                    string imageFileName = "LINELogin_" +Guid.NewGuid().GetHashCode().ToString() + ".jpg"; // Generate a unique image filename
                    string imageFilePath = Path.Combine(_host.WebRootPath, "uploads", imageFileName); // Save to wwwroot/images

                    await System.IO.File.WriteAllBytesAsync(imageFilePath, imageBytes);

                    return imageFileName;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        public IActionResult LineFastLogin()
        {
            return View();
        }

        [HttpPost]//接受view傳過來會員資料進行修改
        public IActionResult LineFastLogin(LineFastLoginVM vm)
        {

            if (!ModelState.IsValid) return View();
            try
            {
                var member = _db.Members.FirstOrDefault(u => u.LineId == u.Email);
                if (member != null)
                {
                    member.Email = vm.Email;
                    _db.SaveChanges();
                }

                TempData["LineFastLogin"] = "前往首頁";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return RedirectToAction("Index");

        }
    }


}
