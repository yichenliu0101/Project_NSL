using Nsl_Core.Models.Dtos.Website;
using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Interfaces;

namespace Nsl_Core.Models.Infra.Repositories.EFRepositories
{
    public class CouponRepositoriesEF:ICouponRepo
    {
        private readonly NSL_DBContext _db;
        public CouponRepositoriesEF(NSL_DBContext db)
        {
            _db = db;
        }
        public CouponDto GetCouponDto(int id)
        {
            var coupon = _db.Coupons.Find(id);
            var dto = new CouponDto()
            {
                Id = coupon.Id,
                Name = coupon.Name,
                StartTime = coupon.StartTime,
                Status = Convert.ToBoolean(coupon.Status),
                ExpireTime = coupon.ExpireTime,
                Description = coupon.Description,
                Condition=coupon.Condition,
                DiscountMoney = coupon.DiscountMoney,
            };

            return dto;
        }
        public int Create(CouponDto dto)
        {
            try
            {
				var coupon = new Coupons()
				{
					Description = dto.Description,
					Name = dto.Name,
					ExpireTime = dto.ExpireTime,
					StartTime = dto.StartTime,
					Status = Convert.ToBoolean(dto.Status),
					CreatedTime = DateTime.Now,
                    Condition = dto.Condition,
					DiscountMoney = dto.DiscountMoney
				};
				_db.Coupons.Add(coupon);
				_db.SaveChanges();

				return coupon.Id;
			}
            catch
            {
                throw new Exception("新增失敗，請聯絡管理員");
            }
        }
        public int Update(CouponDto dto)
        {
            try
            {
				var coupon = _db.Coupons.Find(dto.Id);

				coupon.Name = dto.Name;
				coupon.Description = dto.Description;
				coupon.StartTime = dto.StartTime;
				coupon.ExpireTime = dto.ExpireTime;
                coupon.Status = Convert.ToBoolean( dto.Status);
                coupon.Condition = dto.Condition;
				coupon.DiscountMoney = dto.DiscountMoney;
				coupon.Status = Convert.ToBoolean(dto.Status);

				_db.SaveChanges();

				return coupon.Id;
			}
            catch
            {
                throw new Exception("更新失敗，請聯絡管理員");
            }
        }
    }
}
