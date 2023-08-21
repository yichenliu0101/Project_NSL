using Nsl_Core.Models.ViewModels;

namespace Nsl_Core.Models.Interfaces
{
	public interface IAdminRepo
	{
		int Create(AdminVM vm);
	}
}
