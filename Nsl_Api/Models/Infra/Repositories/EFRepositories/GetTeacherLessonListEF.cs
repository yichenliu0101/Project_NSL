using Microsoft.EntityFrameworkCore;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Interfaces;
using NuGet.DependencyResolver;
using System.Diagnostics.Metrics;

namespace Nsl_Api.Models.Infra.Repositories.EFRepositories
{
    public class GetTeacherLessonListEF : ITeacherLessonRepo
    {
        private NSL_DBContext _db;

        public GetTeacherLessonListEF(NSL_DBContext db)
        {
            _db = db;
        }
        public Task<List<TeacherLessonListDto>> GetTeacherList(lessonSearchDto dto)

        {
            var avgSatisfaction = from s in
                                  (from c in _db.Comments
                                   join mt in _db.MembersTutorRecords on c.MemberTutorRecordId equals mt.Id
                                   join tp in _db.TeachersRealTutorPeriods on mt.TeacherTutorPeriodId equals tp.Id
                                   select new { tp.TeacherId, c.Satisfaction }
                                  )
                                  group s by new
                                  {
                                      s.TeacherId
                                  } into g
                                  select new
                                  {
                                      TeacherId = g.Key.TeacherId,
                                      Avg = g.Average(s => s.Satisfaction),
                                      Cnt = g.Count()
                                  };
            var teacher = from t in _db.TeacherId select t;

            var teachersapply = from ta in _db.TeachersApply select ta;
            if (dto.CategoryId != null)
            {
                teachersapply = teachersapply.Where(o => o.CategoryId == dto.CategoryId);
            }

            var query = (from m in _db.Members
                         join t in teacher on m.Id equals t.MemberId
                         join tr in _db.TeachersResume on t.Id equals tr.TeacherId
                         join l in teachersapply on tr.TeacherId equals l.TeacherId
                         join av in avgSatisfaction on t.Id equals av.TeacherId into avv
                         from av in avv.DefaultIfEmpty()
                         where tr.Title != null && tr.Introduction != null
                         select new TeacherLessonListDto
                         {
                             MemberId = m.Id,
                             TeacherId = t.Id,
                             ImageName = m.ImageName,
                             Title = tr.Title,
                             Price = tr.Price,
                             TeacherName = m.Name,
                             Introduction = tr.Introduction,
                             Satisfaction = av.Avg,
                             CommentCount = av.Cnt,
                         });

            if (string.IsNullOrEmpty(dto.Order)) return query.AsNoTracking().ToListAsync();

            if (dto.Order == "price")
            {
                query = (dto.Flag) ? query.OrderByDescending(o => o.Price) : query.OrderBy(o => o.Price);
            }
            if (dto.Order == "comment")
            {
                query = (dto.Flag) ? query.OrderByDescending(o => o.CommentCount) : query.OrderBy(o => o.CommentCount);
            }
            if (dto.Order == "star")
            {
                query = (dto.Flag) ? query.OrderByDescending(o => o.Satisfaction) : query.OrderBy(o => o.Satisfaction);
            }

            return query.AsNoTracking().ToListAsync();
        }

    }
}
