using Nsl_Core.Models.Dtos.Member.Manager;
using Nsl_Core.Models.Dtos.Teacher.TeacherApply;
using Nsl_Core.Models.Dtos.Teacher.TeacherResume;
using Nsl_Core.Models.Dtos.Website;
using Nsl_Core.Models.EFModels;

namespace Nsl_Core.Models.Interfaces
{
    public interface ITeacherRepo
    {
		TeacherVerifyDto Update(int teacherId);
        int UpdateResume(TeacherEditDto dto);
        int CreateResume(int teacherId);
    }
}
