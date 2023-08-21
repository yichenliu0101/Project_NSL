namespace Nsl_Api.Models.DTOs
{
	public class LINEPayConfirmAmountDto
	{
        public int Count { get; set; }
        public decimal CurrentPrice { get; set; }
        public int? CouponId { get; set; }
        public decimal? DiscountMoney { get; set; }
    }
}
