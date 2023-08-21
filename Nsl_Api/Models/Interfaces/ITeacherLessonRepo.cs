using Nsl_Api.Models.DTOs;

namespace Nsl_Api.Models.Interfaces
{
    public interface ITeacherLessonRepo
    {
        public Task<List<TeacherLessonListDto>> GetTeacherList(lessonSearchDto dto);

    }
}
