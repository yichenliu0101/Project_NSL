namespace Nsl_Core.Models.Dtos.Teacher.TeacherResume
{
    public class TeacherResumeDto
    {
        public int TeacherId { get; set; }
        public int MemberId { get; set; }

        public string? TeacherName { get; set; }

        public string? ImageName { get; set; }

        public int? BankCodeId { get; set; }

        public string? BankCodeName { get; set; }

        public string? BankAccount { get; set; }

        public string? Education { get; set; }

        public string? WorkExperience { get; set; }

        public string? Title { get; set; }

        public string? Introduction { get; set; }      
    }
}
