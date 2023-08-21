using Nsl_Api.Models.EFModels;

namespace Nsl_Api.Models.ViewModels
{
	public class TeacherApplyVM
	{
		public int? MemberId { get; set; }
		public int Category { get; set; }
		public List<int> Language { get; set; }
		public int TutorExperience { get; set; }
		public int WorkStatus { get; set; }
		public int TutorHoursOfWeek { get; set; }
		public int RevenueTarget { get; set; }
		public string? Intro { get; set; }
	}
}
