namespace Nsl_Core.Models.Dtos.Member.Manager
{
    public class MemberCommentDto
    {
        public int Id { get; set; }
        public int MemberTutorRecordId { get; set; }
        public string CommentContent { get; set; }
        public double Satisfaction { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
    }
}
