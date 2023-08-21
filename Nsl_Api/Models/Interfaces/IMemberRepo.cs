using Nsl_Api.Models.DTOs;

namespace Nsl_Api.Models.Interfaces
{
	public interface IMemberRepo
	{
		Task<List<MemberListItemDto>> GetList(string? search, int[]? displayRole);
		Task<string> Delete(int id);
	}
}
