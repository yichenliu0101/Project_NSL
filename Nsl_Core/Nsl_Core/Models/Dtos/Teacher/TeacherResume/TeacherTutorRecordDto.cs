namespace Nsl_Core.Models.Dtos.Teacher.TeacherResume
{
    public class TeacherTutorRecordDto
    {
        public int TeacherId { get; set; }
        public int MemberId { get; set; }

        public string? TeacherName { get; set; }

        public string? ImageName { get; set; }

        public string? CategoryName { get; set; }

    }
}
