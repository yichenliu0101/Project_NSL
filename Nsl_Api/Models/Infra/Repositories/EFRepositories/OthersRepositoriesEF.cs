using Microsoft.EntityFrameworkCore;
using Nsl_Api.Models.Datas;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Interfaces;
using Nsl_Api.Models.Dtos;

namespace Nsl_Api.Models.Infra.Repositories.EFRepositories
{
	public class OthersRepositoriesEF : IOthersRepo
	{
		private NSL_DBContext _db;
		public OthersRepositoriesEF(NSL_DBContext db)
		{
			_db = db;
		}

        public string DeleteShoppingCar(int Id)
        {
            var shopcar = _db.ShoppingCart.Find(Id);
            if (shopcar == null)
            {
                throw new Exception("Cannot Find Data");
            }
            try
            {
                _db.ShoppingCart.Remove(shopcar);
                _db.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                return $"Failed, Message = {ex.Message}";
            }
        }


        public Task<List<Areas>> GetAreas(int cityId)
		{
			return _db.Areas.Where(x => x.CityId == cityId).ToListAsync();
		}

		public Task<List<Citys>> GetCitys()
		{
			return _db.Citys.ToListAsync();
		}

		public Task<List<Categories>> GetCategories(int categoryId)
		{
			return _db.Categories.ToListAsync();
		}

		public Task<List<BankCode>> GetBankCode(int bankCodeId)
		{
			return _db.BankCode.ToListAsync();
		}

		public Task<List<Tags>> GetTags()
		{
			var data = _db.Tags.ToListAsync();

			return data;
		}

		public string CreateTags(OtherTags tags)
		{
			var tagInDb = _db.Tags.Where(x => x.Name != tags.Name).FirstOrDefault();
			if (tagInDb != null) throw new Exception("已經有這個Tag了");

			var tag = new Tags()
			{
				Name = tags.Name,
			};



			_db.Tags.Add(tag);
			_db.SaveChanges();

			return tag.Name;
		}

		public Task<List<TutorPeriod>> GetTutorPeriod()
		{
			return _db.TutorPeriod.ToListAsync();
		}

		public async Task<TeacherApplyDatas> GetApplyDatas()
		{
			return new TeacherApplyDatas()
			{
				Categories = await _db.Categories.ToListAsync(),
				Languages = await _db.Languages.ToListAsync(),
				TutorExperience = await _db.TutorExperience.ToListAsync(),
				WorkStatus = await _db.WorkStatus.ToListAsync(),
				TutorHoursOfWeek = await _db.TutorHoursOfWeek.ToListAsync(),
				RevenueTarget = await _db.RevenueTarget.ToListAsync(),
			};
		}

		public Task<List<PaymentMethods>> GetPay(int paymentId)
		{
			return _db.PaymentMethods.ToListAsync();


		}

        public Task<List<Languages>> GetLangs(int Id)
        {
            return _db.Languages.ToListAsync();
        }
        public Task<List<DeveloperDto>> GetDevelopers()
		{
			var query = from m in _db.Members
						join c in _db.Citys on m.CityId equals c.Id
						join a in _db.Areas on m.AreaId equals a.Id
						where m.Role == 4
						select new DeveloperDto()
						{
							Id = m.Id,
							Name = m.Name,
							Email = m.Email,
							Gender = ((bool)m.Gender) ? "男" : "女",
							Phone = m.Phone,
							ImageName = m.ImageName,
							CityName = c.Name,
							AreaName = a.Name
						};

			return query.ToListAsync();
		}

    }
}
