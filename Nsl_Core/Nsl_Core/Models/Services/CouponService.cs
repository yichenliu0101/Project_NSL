using Nsl_Core.Models.Dtos.Website;
using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Infra.Repositories.EFRepositories;
using Nsl_Core.Models.Interfaces;

namespace Nsl_Core.Models.Services
{
	public class CouponService
	{
		private readonly NSL_DBContext _db;

		public CouponService(NSL_DBContext db)
		{
			_db = db;
		}
		public int Create(CouponDto dto)
		{
			try
			{
				ICouponRepo repo = new CouponRepositoriesEF(_db);
				int newId = repo.Create(dto);

				return newId;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public int Update(CouponDto dto)
		{
			try
			{
				ICouponRepo repo = new CouponRepositoriesEF(_db);
				int newId = repo.Update(dto);

				return newId;
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}
	}
}
