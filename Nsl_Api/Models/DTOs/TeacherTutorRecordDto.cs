namespace Nsl_Api.Models.DTOs
{
    public class TeacherTutorRecordDto
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
		public DateTime? TutorStartTime { get; set; }
        public string? TeacherName { get; set; }

        public string? MemberName { get; set; }

        public string? CategoryName { get; set; }

        public double? TutorStatus { get; set; }

        public string? StatusName { get; set; }

        public double? Satisfaction { get; set; }

        public string? CommentContent { get; set; }

    }
}
