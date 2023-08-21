namespace Nsl_Api.Models.DTOs
{
    public class ChatsDto
    {
        public int SenderId { get; set; }
        public string SenderName { get; set;}
        public int RecipientId { get; set; }
        public string RecipientName { get; set; }
        public string MessageText { get; set;}
        public DateTime CreateTime { get; set; }
    }
}
