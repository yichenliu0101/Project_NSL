namespace Nsl_Core.Models.Dtos.Member.Manager
{
    public class MemberLinePayConfirmDto
    {
        public MemberLinePayConfirmDto() 
        {
            Currency = "TWD";
        }
        public decimal Amount { get; set; }
        public string Currency { get; }
    }
}
