namespace Nsl_Api.Models.DTOs
{
    public class TeacherStudentDto
    {
        public int MemberId { get; set; }
        public string? MemberName { get; set; }

        public string? MemberImageName { get; set; }

        public string? Email {get; set; }

        public int? TrueTutor { get; set; }

        public int? FalseTutor { get; set; }

        public double? AvgSatisfaction { get; set; }
    }
}
