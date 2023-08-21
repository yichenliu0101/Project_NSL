using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Infra.Repositories.EFRepositories;
using Nsl_Core.Models.Interfaces;
using Nsl_Core.Models.ViewModels;
using System.Data;

namespace Nsl_Core.Models.Services
{
	public class AdminService
	{
		private readonly NSL_DBContext _context;

		public AdminService(NSL_DBContext context)
		{
			_context = context;
		}
		public int Create(AdminVM vm)
		{
			var emailInDb = _context.Members.FirstOrDefault(x=>x.Email == vm.Email);

			if(emailInDb != null) throw new DuplicateNameException(vm.Email);

			IAdminRepo repo = new AdminRepositoriesEF(_context);
			try
			{
				int newId = repo.Create(vm);
				return newId;
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);	
			}
		}
	}
}
