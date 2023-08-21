using System.Diagnostics.Contracts;

namespace Nsl_Api.Models.DTOs
{
    public class GetAllShopListDto
    {
        public string Ordercode { get; set; }
        public string TeacherName { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public decimal CurrentPrice { get; set; }
        public string CouponName { get; set; }
        public string PaymentName { get; set; }
        public decimal DiscountMoney { get; set; }
        public decimal AllPrice { get; set; }
        public DateTime ConsumeTime { get; set; }
    }
}
