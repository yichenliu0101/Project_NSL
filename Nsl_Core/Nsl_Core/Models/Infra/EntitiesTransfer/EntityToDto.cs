using Nsl_Core.Models.Dtos.Member.Login;
using Nsl_Core.Models.Dtos.Teacher.TeacherApply;
using Nsl_Core.Models.EFModels;

namespace Nsl_Core.Models.Infra.EntitiesTransfer
{
    public static class EntityToDto
    {
        public static TeacherVerifyDto ToTeacherVerifyDto(this Members entity)
        {
            return new TeacherVerifyDto
            {
                MemberId = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Gender = entity.Gender.HasValue ? (entity.Gender.Value ? "先生" : "女士") : "會員"
            };
        }
        public static LoginDto ToLoginDto(this Members entity)
        {
            return new LoginDto
            {
                Id = entity.Id,
                ImageName = entity.ImageName,
                Name = entity.Name,
                Role = entity.Role
            };
        }
    }
}
