using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.ViewModels;
using Nsl_Api.Models.Dtos;

namespace Nsl_Api.Models.Interfaces
{
    public interface ITeacherRepo
    {
        //Task<List<TeacherTutorRecordDto>> GetTutorRecord(int id);
        Task<List<TeacherApplyListDto>> GetAllApplyList(string? search, int? searchLan);

		Task<List<TeacherTagsDto>> GetTeacherTags(int teacherId);

		int CreateTeacherTags(TeacherCreate dto);

        string DeleteTeachersTags(TeachersTagsVM vm);
        string DeleteApply(int teacherId);

        string DeleteComment(int id);

        string DeleteResume(int teacherId);

        Task<string> Update(int id);

		int EditTeacherTags(TeacherTagsDto dto);

		decimal? EditTeacherPrice(TeacherPriceVM vm);

		Task<List<Tags>> GetAutoCompleteTags(string input);
	}
}
