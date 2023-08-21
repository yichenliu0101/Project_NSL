using Microsoft.AspNetCore.Mvc;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.ViewModels;

namespace Nsl_Api.Models.Interfaces
{
    public interface ITutorRepo
    {
        Task<List<TutorPeriodFullCalendarDto>> GetTutorPeriodData(int teacherId);
        Task<List<TutorPeriodFullCalendarDto>> GetMemberTutorPeriodRecord(int memberId);
        Task<List<TutorPeriodFullCalendarDto>> GetTeacherTutorPeriodData(int teacherId);
        string CreateNewCourse(TeachersRealTutorPeriods entity);
        string DeleteCourse(TeacherDeleteCourseVM vm);
        string EditDefaultTutorPeriod(TeacherDefaultPeriodVM vm);
		Task<TeacherDefaultPeriodDto> GetDefaultTutorPeriod(int teacherId);
        string MemberSelectCourse(MemberSelectCourseVM vm);
		Task<List<MemberTutorStockDto>> GetTutorStock(int memberId);
	}
}
