using Microsoft.CodeAnalysis.Rename;
using Microsoft.EntityFrameworkCore;
using Nsl_Core.Models.Dtos.Member.Manager;
using Nsl_Core.Models.Dtos.Teacher.TeacherApply;
using Nsl_Core.Models.Dtos.Teacher.TeacherResume;
using Nsl_Core.Models.Dtos.Website;
using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Infra.EntitiesTransfer;
using Nsl_Core.Models.Interfaces;
using Nsl_Core.Models.ViewModels;

namespace Nsl_Core.Models.Infra.Repositories.EFRepositories
{
    public class TeacherRepositoryEF : ITeacherRepo
    {
        private readonly NSL_DBContext _context;

        public TeacherRepositoryEF(NSL_DBContext context)
        {
            _context = context;
        }
        public int UpdateResume(TeacherEditDto dto)
        {
            try
            {
                var resume = _context.TeachersResume.Find(dto.teacherId);
                resume.BankCodeId = dto.BankCodeId;
                resume.BankAccount = dto.BankAccount;
                resume.Education = dto.Education;
                resume.WorkExperience = dto.WorkExperience;
                resume.Title = dto.Title;
                resume.Introduction = dto.Introduction;
                resume.ModifiedTime = DateTime.Now;


                _context.SaveChanges();

                return _context.TeachersResume.Count(x => x.TeacherId == dto.teacherId);
            }
            catch (Exception)
            {
                throw new Exception("資料錯誤");
            }

        }

        public int CreateResume(int teacherId)
        {
            try
            {
                var teacher = new TeachersResume()
                {
                    TeacherId = teacherId,
                };

                _context.TeachersResume.Add(teacher);
                _context.SaveChanges();

                return teacher.TeacherId;
            }
            catch (Exception)
            {
                throw new Exception("資料錯誤");
            }
        }

        public TeacherVerifyDto Update(int teacherId)
        {
            try
            {
                var teacher = _context.TeacherId.Find(teacherId);
                if (teacher == null) throw new ArgumentNullException("找不到老師ID");

                var tMemberInfo = _context.Members.Find(teacher.MemberId);
                if (tMemberInfo == null) throw new ArgumentNullException("沒有此會員");

                tMemberInfo.Role = 2;
                _context.SaveChanges();

                var info = tMemberInfo.ToTeacherVerifyDto();
                return info;
            }
            catch (Exception)
            {
                throw new Exception("資料錯誤");
            }
        }
    }
}
