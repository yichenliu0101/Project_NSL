namespace Nsl_Api.Models.DTOs
{
	public class TeacherCommentsDto
	{
		public int Id { get; set; }

		public string? MemberName { get; set; }

		public double? Satisfaction { get; set; }

		public string? CommentContent { get; set; }
	}
}
