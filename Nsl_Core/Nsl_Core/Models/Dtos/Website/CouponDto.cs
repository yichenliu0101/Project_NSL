namespace Nsl_Core.Models.Dtos.Website
{
    public class CouponDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public decimal DiscountMoney { get; set; }
        public int? Condition { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
