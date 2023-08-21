namespace Nsl_Api.Models.DTOs
{
	public class TeacherDefaultPeriodDto
	{
		public TeacherDefaultPeriodDto() 
		{
			Mon = new List<int>();
			Tue = new List<int>();
			Wed = new List<int>();
			Thu = new List<int>();
			Fri = new List<int>();
			Sat = new List<int>();
			Sun = new List<int>();
		}
		public List<int>? Mon { get; set; }
		public List<int>? Tue { get; set; }
		public List<int>? Wed { get; set; }
		public List<int>? Thu { get; set; }
		public List<int>? Fri { get; set; }
		public List<int>? Sat { get; set; }
		public List<int>? Sun { get; set; }
	}
}
