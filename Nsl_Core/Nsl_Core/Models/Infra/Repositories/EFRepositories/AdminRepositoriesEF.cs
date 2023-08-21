using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Infra.EntitiesTransfer;
using Nsl_Core.Models.Interfaces;
using Nsl_Core.Models.ViewModels;

namespace Nsl_Core.Models.Infra.Repositories.EFRepositories
{
    public class AdminRepositoriesEF : IAdminRepo
	{
		private readonly NSL_DBContext _context;

		public AdminRepositoriesEF(NSL_DBContext context)
		{
			_context = context;
		}
		public int Create(AdminVM vm)
		{
			var entity = vm.AdminVMToMember();

			_context.Members.Add(entity);
			_context.SaveChanges();

			return _context.Members.OrderBy(x=>x.Id).Last().Id;
		}
	}
}
