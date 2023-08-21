using Nsl_Api.Models.EFModels;
using System.ComponentModel;

namespace Nsl_Api.Models.DTOs
{
    public class ConsumptionRecords
    {

        public int MemberId { get; set; }
        public int PaymentId { get; set; }
        public int? CouponId { get; set; }
        public List<ConsumptionRecordsDetail> Detail { get; set; }
        public List<int>ShoppingCarId { get; set; }
        public decimal? DiscountMoney { get; set; }
        public string? OrderCode { get; set; }
      
        
    }
}
