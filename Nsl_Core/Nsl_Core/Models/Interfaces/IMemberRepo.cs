using Nsl_Core.Models.Dtos.Member.Manager;

namespace Nsl_Core.Models.Interfaces
{
    public interface IMemberRepo
    {
        MemberDto Get(int? memberId);
        int Create(MemberDto dto);
        int Update(MemberDto dto);

        List<MemberConsumerRecordDto> GetList(int? memberId);

        List<MemberGetDetailDto> GetDetail(string? ordercode);

    }
}
