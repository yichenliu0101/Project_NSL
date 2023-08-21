namespace Nsl_Core.Models.Dtos.Member.Manager
{
    public class MemberConsumerRecordDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string? OrderCode { get; set; }
        public DateTime? ConsumeTime { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal? Total { get; set; }
    }
}
