namespace Nsl_Api.Models.Dtos
{
    public class TeacherApplyListDto
    {
        public int Id { get; set; }

        public string? TeacherName { get; set; }

        public int? CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public bool TutorStatus { get; set; }

        public string? StatusName { get; set; }

        public string? ImageName { get; set; }

        public string? Intro { get; set; }

    }
}
