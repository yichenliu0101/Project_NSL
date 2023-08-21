namespace Nsl_Api.Models.DTOs
{
    public class TeacherCreate
    {
        public int TeacherId { get; set; }

        public int TagsId { get; set; }

        public string? TagsName { get; set; }
    }
}
