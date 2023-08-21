using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nsl_Core.Models.Dtos.Member.Login;
using Nsl_Core.Models.Infra.Repositories.EFRepositories;
using Nsl_Core.Models.Infra;
using Nsl_Core.Models.Interfaces;
using Nsl_Core.Models.EFModels;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using System.Runtime.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Humanizer;
using MemoryCache = System.Runtime.Caching.MemoryCache;
using Nsl_Core.Models.Dtos.Member.Manager;
using XAct;

namespace Nsl_Core.Controllers.Front
{
    public class CartController : Controller
    {
        private readonly NSL_DBContext _context;
        private readonly IWebHostEnvironment _host;
		private readonly IConfiguration _configuration;


		public CartController(NSL_DBContext context, IWebHostEnvironment host, IConfiguration configuration)
        {
            _context = context;
            _host = host;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OrderView()
        {
            return View();
        }

        public IActionResult PurchaseDetails(string Id)
        {
            ViewBag.Ordercode = Id;
            return View();
        }

		public IActionResult ToEcpay(string orderCode)
		{
            var ecpay = new Ecpay(_configuration);
			var orderData = ecpay.GetOrderData(orderCode);
			var toEcpayData = ecpay.GetProductOrder(orderData);

			return PartialView(toEcpayData);
		}
        public IActionResult OrderListUpdate(string orderCode= "3a354bd6114046c49db9")
        {
            ViewBag.Ordercode = orderCode;
			return PartialView();
        }

        public IActionResult ToLinePay()
        { 
            return PartialView();
        }

		public IActionResult PayInfo(string Id)
		{
			var ecpay = new Ecpay(_configuration);
			var data= ecpay.GetPayInfo(Id);
			return View(data);
		}
	}
}
