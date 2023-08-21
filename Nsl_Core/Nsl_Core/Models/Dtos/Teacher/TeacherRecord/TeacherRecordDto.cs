namespace Nsl_Core.Models.Dtos.Teacher.TeacherRecord
{
    public class TeacherRecordDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? BankAccount { get; set; }
        public decimal? Price { get; set; }
        public string? WorkExperience { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? FinishCount { get; set; }
        public int? UnfinishedCount { get; set; }
        public double? Satisfaction { get; set; }
    }
}
