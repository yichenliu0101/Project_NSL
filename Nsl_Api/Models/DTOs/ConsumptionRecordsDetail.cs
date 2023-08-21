namespace Nsl_Api.Models.DTOs
{
    public class ConsumptionRecordsDetail
    {
        public int Id { get; set; }
        public int ConsumptionRecordsId { get; set;}
        public int TeacherId { get; set; }   
        public int Count { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}
