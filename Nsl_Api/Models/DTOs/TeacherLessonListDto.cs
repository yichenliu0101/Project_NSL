using System.Security.Policy;

namespace Nsl_Api.Models.DTOs
{
    public class TeacherLessonListDto
    {
        
        public int MemberId { get; set; }

        public int TeacherId { get; set; }

        public string? ImageName { get; set; }

        public string? TeacherName { get; set;}

        public string? Title { get; set; }  

        public decimal? Price { get; set; }

        public string? Introduction { get; set; }

        public double? Satisfaction{ get; set; }
        public int? CommentCount { get; set; }
        public List<string>? TagNames { get; set; }
    }
}
