using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Infra.LinePay.Domain;
using Nsl_Api.Models.Infra.LinePay.Dtos;

namespace Nsl_Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class LinePayController : ControllerBase
    {
        private readonly LinePayService _linePayService;
        private readonly NSL_DBContext _db;
        private readonly IConfiguration _configuration;

        public LinePayController(NSL_DBContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
            _linePayService = new LinePayService(_db, _configuration);
        }

        [HttpPost("Create")]
        public async Task<PaymentResponseDto> CreatePayment(ConsumptionRecords dto)
        {
            return await _linePayService.SendPaymentRequest(dto);
        }

        [HttpPost("Confirm")]
        public async Task<PaymentConfirmResponseDto> ConfirmPayment([FromQuery] string transactionId, [FromQuery] string orderId, PaymentConfirmDto dto)
        {
            return await _linePayService.ConfirmPayment(transactionId, orderId, dto);
        }

        [HttpGet("Cancel")]
        public async void CancelTransaction([FromQuery] string transactionId)
        {
            _linePayService.TransactionCancel(transactionId);
        }

        [HttpGet("GetAmount")]
        public async Task<decimal> GetAmount(string orderId)
        {
			var order = await (from mcr in _db.MembersConsumptionRecords
						 join mcrd in _db.MembersConsumptionRecordDetails on mcr.Id equals mcrd.MembersConsumptionRecordId
						 join c in _db.Coupons on mcr.CouponId equals c.Id into cp
						 from cc in cp.DefaultIfEmpty()
						 where mcr.OrderCode == orderId
						 select new LINEPayConfirmAmountDto() { Count = mcrd.Count, CurrentPrice = mcrd.CurrentPrice, CouponId = mcr.CouponId, DiscountMoney = cc.DiscountMoney }).ToListAsync();

			decimal total = order.Sum(x => x.Count * x.CurrentPrice);
			return total;
		}
    }
}
