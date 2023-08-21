using Nsl_Core.Models.Dtos.Member.Manager;

namespace Nsl_Core.Models.Interfaces
{
    public interface IMemberConsumeRecodRepo 
    {
       List<MemberConsumerRecordDto> GetList(int? memberId);

       List<MemberGetDetailDto> GetDetail(string? ordercode);
    }
}
