namespace Nsl_Core.Models.Dtos.Member.Manager
{
    public class MemberTutorRecordDto
    {
        public int MemberTutorPeriodId { get; set; }
        public string CategoryName { get; set; }
        public DateTime TutorStartDate { get; set; }
        public int CourseCount { get; set; }
        public string TeacherName { get; set; }
        public bool StatusName { get; set; }
        public double Satisfaction { get; set; }
        public int TeacherId { get; set; }
        public string CommentContent { get; set; }
    }
}
