namespace Nsl_Core.Models.Dtos.Teacher.TeacherResume
{
    public class BackTeacherResumeDto
    {
        public int TeacherId { get; set; }
        public int MemberId { get; set; }
        public string? Name { get; set; }
        public string? ImageName { get; set; }
        public string? BankCodeName { get; set; }
        public string? BankAccount { get; set; }
        public string? WorkExperience { get; set; }
        public decimal? Price { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? FinishCount { get; set; }
        public int? UnfinishedCount { get; set; }
        public double? Satisfaction { get; set; }
    }
}
