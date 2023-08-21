using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Nsl_Core.Models;
using Nsl_Core.Models.Infra.Repositories.EFRepositories;
using Nsl_Core.Models.Interfaces;
using Nsl_Core.Models.Services;
using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Dtos.Website;
using XAct;

namespace NSL_html.Controllers.Back
{
    public class BackMaintainController : Controller
    {
        private readonly IWebHostEnvironment _host;
        private readonly NSL_DBContext _db;
        public BackMaintainController(IWebHostEnvironment host, NSL_DBContext db)
        {
            _host = host;
            _db = db;
        }
        //索引
        public IActionResult Index()
        {
            return PartialView();
        }

        //Article
        public IActionResult ArticleIndex()
        {
            return PartialView();
        }
        public IActionResult ArticleUpdate(int id)
        {

            IArticleRepo repo = new ArticleRepositoriesEF(_host, _db);
            var dto = repo.GetArticle(id);

            return PartialView(dto);
        }

        [HttpPost]
        public IActionResult ArticleUpdate(ArticleDto dto, IFormFile Picture)
        {
            try
            {
                var service = new ArticleService(_host, _db);
                service.Update(dto, Picture);
            }
            catch(Exception ex)
            {
                TempData.Add("ErrMessage", $"更新失敗 {ex.Message}");
            }

            return RedirectToAction("ArticleIndex");
        }
        public IActionResult ArticleCreate()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult ArticleCreate(ArticleDto dto, IFormFile Picture)
        {
            try
            {
                var service = new ArticleService(_host, _db);
                service.Create(dto, Picture);
            }
            catch (Exception ex)
            {
                TempData.Add("ErrMessage", $"更新失敗 {ex.Message}");
                return PartialView();
            }

            return RedirectToAction("ArticleIndex");
        }


        //Coupon
        public IActionResult CouponIndex()
        {
            return PartialView();
        }
        public IActionResult CouponUpdate(int id)
        {
            try
            {
                ICouponRepo repo = new CouponRepositoriesEF(_db);
                var dto = repo.GetCouponDto(id);

                return PartialView(dto);
            }
            catch (Exception ex)
            {
                TempData.Add("ErrMessage", $"網頁失效，此項目可能已被刪除");

                return RedirectToAction("CouponIndex");
            }
        }
        [HttpPost]
        public IActionResult CouponUpdate(CouponDto coupons)
        {
            try
            {
                var service = new CouponService(_db);
                service.Update(coupons);
            }
            catch (Exception ex)
            {
                TempData.Add("ErrMessage", $"更新失敗 {ex.Message}");
            }

            return RedirectToAction("CouponIndex");
        }
        public IActionResult CouponCreate()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult CouponCreate(CouponDto coupons)
        {
            try
            {
                var service = new CouponService(_db);
                service.Create(coupons);
            }
            catch (Exception ex)
            {
                TempData.Add("ErrMessage", $"新增失敗 {ex.Message}");
            }

            return RedirectToAction("CouponIndex");
        }
    }
}
