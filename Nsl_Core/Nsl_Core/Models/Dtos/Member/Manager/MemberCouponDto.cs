namespace Nsl_Core.Models.Dtos.Member.Manager
{
    public class MemberCouponDto
    {
        public int Id { get; set; }
        public string? Name { get;set; }
        public string? Description { get; set; } 
        public DateTime? ExpireTime { get; set; }

        public decimal DiscountMoney { get; set; }

    }

}
