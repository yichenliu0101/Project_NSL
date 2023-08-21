using Nsl_Core.Models.Dtos.Website;

namespace Nsl_Core.Models.Interfaces
{
    public interface ICouponRepo
    {
        int Create(CouponDto dto);
        int Update(CouponDto dto);
        CouponDto GetCouponDto(int id);
    }
}
