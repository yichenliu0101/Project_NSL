using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Nsl_Core.Models.EFModels;
using System.Configuration;
using Nsl_Core.Models.Infra;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
builder.Services.AddDbContext<NSL_DBContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("NSLDbContext")
));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(option =>
	{
		option.SlidingExpiration = true;
		option.Cookie.Expiration = TimeSpan.FromDays(7);
		option.Cookie.MaxAge = TimeSpan.FromDays(7);
		option.Cookie.HttpOnly = true;
		option.Cookie.SecurePolicy = CookieSecurePolicy.Always;
	});
	
builder.Services.AddSingleton<JwtHelpers>();
builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.IncludeErrorDetails = true; // �����ҥ��ѮɡA�|��ܥ��Ѫ��Բӿ��~��]

		options.TokenValidationParameters = new TokenValidationParameters
		{
			// ñ�o��
			ValidateIssuer = true,
			ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),
			// ������
			ValidateAudience = false,
			ValidAudience = "JwtAuthDemo",
			// Token �����Ĵ���
			ValidateLifetime = true,
			// �p�G Token ���]�t key �~�ݭn���ҡA�@�볣�u��ñ���Ӥw
			ValidateIssuerSigningKey = false,
			// key
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SignKey")))
		};
	});

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll",
		builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
builder.Services.AddAuthentication()
        .AddFacebook(options =>
        {
            options.AppId = "169050442808007";
            options.AppSecret = "da9b7291dfd7b83e2eb91c1c2dd074ce";
        });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseStaticFiles();
app.MapHub<ChatHub>("/chatHub");
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=NSL}/{action=Login}/{id?}");
app.Run();
