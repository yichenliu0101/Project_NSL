using Dapper;
using Microsoft.Data.SqlClient;
using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Dtos.Member.Manager;
using NuGet.DependencyResolver;
using System.Diagnostics.Metrics;
using System.Configuration;
using XAct;

namespace Nsl_Core.Models.Infra.Repositories.DapperRepositories
{
    public class MemberRepo
    {
       
        private readonly SqlConnection _conn;
        public MemberRepo(IConfiguration configuration )
        {
            _conn = new SqlConnection(configuration.GetConnectionString("NSLDbContext"));
            
        }

        public List<MemberTutorRecordDto> GetTutorRecords(int memberid)
        {
            var sql = @"SELECT mtr.Id AS MemberTutorPeriodId,oc.Name AS CategoryName,CONVERT(DATE, CONVERT(DATE, tt.TutorStartTime)) AS TutorStartDate,
                        t.Id AS TeacherId,m.Name AS TeacherName,mtr.Status AS StatusName,cc.Satisfaction AS Satisfaction  ,COUNT(distinct tt.TutorStartTime) AS CourseCount,cc.CommentContent AS CommentContent 
                        FROM Member.Members m 
                        JOIN Teacher.TeacherId t 
                        ON t.MemberId = m.Id 
                        JOIN Teacher.TeachersApply ta 
                        ON ta.TeacherId = t.Id 
                        JOIN Teacher.TeachersRealTutorPeriods tt 
                        ON tt.TeacherId = t.Id 
                        JOIN Member.MembersTutorRecords mtr 
                        ON mtr.TeacherTutorPeriodID = tt.Id 
                        LEFT JOIN Comment.Comments cc
                        ON cc.MemberTutorRecordId = mtr.Id 
                        JOIN Others.Categories oc 
                        ON oc.Id = ta.CategoryId 
                        JOIN Member.Members m2 
                        ON m2.Id = mtr.MemberId 
                        WHERE m2.Id = @Id
                        GROUP BY mtr.Id,oc.Name,m.Name,mtr.Status,cc.Satisfaction,tt.TutorStartTime,t.id,cc.CommentContent  
                        ORDER BY tt.TutorStartTime,m.Name";
            var results = _conn.Query<MemberTutorRecordDto>(sql, new { Id = memberid }).ToList();


            var list=new List<MemberTutorRecordDto>();
            
            DateTime sameday = new DateTime();
            for (int i=0;i<results.Count(); i++)
            {
                if (sameday== results[i].TutorStartDate)
                {
                    list[i - 1].CourseCount++;
                }
                else
                {
                    var dto = new MemberTutorRecordDto();
                    sameday = results[i].TutorStartDate;
                    dto.MemberTutorPeriodId = results[i].MemberTutorPeriodId;
                    dto.CategoryName = results[i].CategoryName;
                    dto.TutorStartDate = results[i].TutorStartDate;
                    dto.CourseCount = results[i].CourseCount;
                    dto.TeacherId = results[i].TeacherId;
                    dto.TeacherName = results[i].TeacherName;
                    dto.StatusName = results[i].StatusName;
                    dto.Satisfaction = results[i].Satisfaction;
                    list.Add(dto);
                }
            }
            return list;
        }
    }
}
