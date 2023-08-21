using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nsl_Core.Models.Dtos;
using Nsl_Core.Models.EFModels;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nsl_Core.Models.Dtos.Member.Login;
using System.Net;
using System.Net.Http;

namespace Nsl_Core.Models.Infra
{
    public class MemberVerify : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
			var httpContext = context.HttpContext;
			string? token = context.HttpContext.Request.Cookies.Get<string>("Login");
			LoginDto? user = JsonSerializer.Deserialize<LoginDto>(JwtHelpers.DecodeToken(token));
			if (user == null)
            {
				httpContext.Session.SetString("RoleVerify", "請登入帳號");
				context.Result = new RedirectToActionResult("Login", "NSL", null);
            }
			if(user.EmailCheck==false)
            {
				httpContext.Session.SetString("RoleVerify", "帳號未驗證");
				context.Result = new RedirectToActionResult("Login", "NSL", null);
			}
        }
    }

    public class NotMemberVerify : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string? token = context.HttpContext.Request.Cookies.Get<string>("Login");
            if (token != null)
            {
                context.Result = new RedirectToActionResult("Index", "NSL", null);
            }
        }
    }

    public class AdminVerify : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
			var httpContext = context.HttpContext;
			string? token = context.HttpContext.Request.Cookies.Get<string>("Login");
			LoginDto? user = JsonSerializer.Deserialize<LoginDto>(JwtHelpers.DecodeToken(token));
			if (user == null||user.EmailCheck==false)
            {
                httpContext.Session.SetString("RoleVerify", "請登入帳號");
                context.Result = new RedirectToActionResult("Login", "NSL", null);
            }
			if (user.EmailCheck == false)
			{
				httpContext.Session.SetString("RoleVerify", "帳號未驗證");
				context.Result = new RedirectToActionResult("Login", "NSL", null);
			}
			if (user.Role < 3)
			{
				httpContext.Session.SetString("RoleVerify", "請重新操作");
				context.Result = new RedirectToActionResult("Index", "NSL", null);
			}
		}
    }

    public class TeacherVerify : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
			var httpContext = context.HttpContext;
			string? token = context.HttpContext.Request.Cookies.Get<string>("Login");
			LoginDto? user = JsonSerializer.Deserialize<LoginDto>(JwtHelpers.DecodeToken(token));
			if (user == null)
            {
                httpContext.Session.SetString("RoleVerify", "請登入帳號");
                context.Result = new RedirectToActionResult("Index", "NSL", null);
            }
			if (user.EmailCheck == false)
			{
				httpContext.Session.SetString("RoleVerify", "帳號未驗證");
				context.Result = new RedirectToActionResult("Login", "NSL", null);
			}
			if (user.Role < 2)
			{
				httpContext.Session.SetString("RoleVerify", "請重新操作");
				context.Result = new RedirectToActionResult("Index", "NSL", null);
			}
		}
    }
 }
