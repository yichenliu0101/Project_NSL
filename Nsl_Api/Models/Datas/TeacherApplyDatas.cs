using Nsl_Api.Models.EFModels;

namespace Nsl_Api.Models.Datas
{
	public class TeacherApplyDatas
	{
		public List<Categories>? Categories { get; set; }
		public List<Languages>? Languages { get; set; }
		public List<TutorExperience>? TutorExperience { get; set; }
		public List<WorkStatus>? WorkStatus { get; set; }
		public List<TutorHoursOfWeek>? TutorHoursOfWeek { get; set; }
		public List<RevenueTarget>? RevenueTarget { get; set; }
	}
}
