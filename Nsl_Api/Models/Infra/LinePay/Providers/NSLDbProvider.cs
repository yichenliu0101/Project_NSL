using Microsoft.CodeAnalysis.CSharp;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Infra.LinePay.Dtos;
using static System.Net.WebRequestMethods;

namespace Nsl_Api.Models.Infra.LinePay.Providers
{
    public class NSLDbProvider
    {
        private readonly NSL_DBContext _db;
        public NSLDbProvider(NSL_DBContext db) 
        {
            _db = db;
        }
        public PaymentRequestDto ToRequest(ConsumptionRecords dto)
        {
            var requestDto = new PaymentRequestDto() { Packages = new List<PackageDto>(), RedirectUrls = new RedirectUrlsDto() };
            int total = 0;
			requestDto.Currency = "TWD";
			requestDto.OrderId = dto.OrderCode;

			var package = new PackageDto() { Products = new List<LinePayProductDto>() };
            package.Id = DateTime.Now.ToString("yyyyMMddIHHmmss");
            package.Name = "NSL";
			foreach (var item in dto.Detail)
            {
				var product = new LinePayProductDto();
                product.Id = item.TeacherId.ToString();
                string name = (from t in _db.TeacherId
							   join m in _db.Members on t.MemberId equals m.Id
							   where t.Id == item.TeacherId
							   select m).FirstOrDefault().Name;

				product.Name = name;

                product.Quantity = item.Count;
                product.Price = item.CurrentPrice;
                package.Products.Add(product);
                total += (int)item.CurrentPrice * item.Count;
            }
			requestDto.Amount = total;
            package.Amount = total;
            requestDto.Packages.Add(package);

			requestDto.RedirectUrls.ConfirmUrl = "https://localhost:7217/Cart/ToLinePay";
            requestDto.RedirectUrls.CancelUrl = "https://8fe5-125-227-255-79.ngrok-free.app/api/LinePay/Cancel";
            requestDto.RedirectUrls.ConfirmUrlType = "SERVER";

			return requestDto;
        }
	}
}
