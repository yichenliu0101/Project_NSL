using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Nsl_Core.Models.Dtos.Member.Login;
using NuGet.Common;

namespace Nsl_Core.Models.Infra
{
	public static class CookiesInfra
	{
		public static void Set<T>(this IResponseCookies cookies,string key, T value)
		{
            cookies.Append(key, JsonSerializer.Serialize(value));
		}

		public static T Get<T>(this IRequestCookieCollection cookies, string key)
		{
			cookies.TryGetValue(key, out string value);
			return (value == null) ? default : JsonSerializer.Deserialize<T>(value);
		}
	}
}
