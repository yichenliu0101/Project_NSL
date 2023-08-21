namespace Nsl_Core.Models.Dtos.Teacher.TeacherTutorRecord
{
    public class TeacherTutorRecordDto
    {
        public int TeacherId { get; set; }
        public int MemberId { get; set; }
        public string? TeacherName { get; set; }
        public string? MemberName { get; set; }
        public string? ImageName { get; set; }
    }
}
