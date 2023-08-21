namespace Nsl_Core.Models.Dtos.Teacher.TeacherResume
{
    public class TeacherEditDto
    {
        public int teacherId { get; set; }

        public int memberId { get; set; }
        public int? BankCodeId { get; set; }
        public string? BankCodeName { get; set; }

        public string? BankAccount { get; set; }

        public string? Education { get; set; }

        public string? WorkExperience { get; set; }

        public string? Title { get; set; }

        public string? Introduction { get; set; }

        public string Msg { get; set; }
    }
}
