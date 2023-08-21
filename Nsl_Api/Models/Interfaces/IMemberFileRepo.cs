using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.Dtos;

namespace Nsl_Api.Models.Interfaces
{
    public interface IMemberFileRepo
    {
        Task<List<MemberFileDto>> GetMemberFile(int memberId);

        Task<List<MemberDetailDto>> GetMemberDetail(int? consumeId);
    }
}
