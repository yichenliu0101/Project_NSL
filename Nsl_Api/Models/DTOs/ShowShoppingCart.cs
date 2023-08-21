namespace Nsl_Api.Models.DTOs
{
    public class ShowShoppingCart
    {   public int Id { get; set; }
        public int MemberId { get; set; }
        public int TeacherId { get; set; }
        public int Count { get; set; }
        public string Title { get; set; }
        public decimal? Price { get; set; }
        public string ImageName { get; set; }

        
    }
}
