namespace Nsl_Api.Models.ViewModels
{
	public class TeacherDefaultPeriodVM
	{
        public int TeacherId { get; set; }
        public Dictionary<int, List<int>> WeekPeriod { get; set; }
    }
}
