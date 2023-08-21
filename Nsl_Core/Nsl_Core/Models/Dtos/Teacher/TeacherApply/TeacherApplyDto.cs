namespace Nsl_Core.Models.Dtos.Teacher.TeacherApply
{
    public class TeacherApplyDto
    {
        public int Id { get; set; }

        public string? TeacherName { get; set; }
        public string? ImageName { get; set; }

        public string? CategoryName { get; set; }

        public string? LanguageName { get; set; }

        public string? TutorExperienceName { get; set; }

        public string? WorkStatusName { get; set; }

        public string? TutorHoursOfWeekName { get; set; }

        public string? RevenueTargetName { get; set; }

        public string? Intro { get; set; }
    }
}
