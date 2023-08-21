using Nsl_Api.Models.EFModels;

namespace Nsl_Api.Models.DTOs
{
	public class MemberListItemDto
	{
		public int MemberId { get; set; }
		public string? Name { get; set; }
		public string? Gender { get; set; }
		//public string GenderString { get; set; }
		public DateTime? Birthday { get; set; }
		public string? Email { get; set; }
		public string? CityName { get; set; }
		public string? AreaName { get; set; }
		public Roles Role { get; set; }
		public string? ImageName { get; set; }

	}
}
