using Dapper;
using Microsoft.Data.SqlClient;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Dtos;
using System.Drawing;

namespace Nsl_Api.Models.Infra.Repositories.DapperRepositories
{
	public class TeacherRepositoriesDapper
	{
        private readonly SqlConnection _conn;
        public TeacherRepositoriesDapper(IConfiguration configuration) 
        {
			_conn = new SqlConnection(configuration.GetConnectionString("NSLDbContext"));
		}
		public async Task<List<TeacherApplyListDto>> GetAllApplyList(string? search, string searchLan)
		{
			var sql = "select ta.TeacherId as Id,m.Name as TeacherName, c.Name as CategoryName, m.ImageName, s.name as StatusName , Intro\r\nfrom[Teacher].[TeacherId]tid\r\njoin[Member].[Members] m on tid.MemberId = m.Id\r\njoin[Teacher].[TeachersApply]ta on tid.Id = ta.TeacherId\r\njoin[Others].[Categories]c on ta.CategoryId = c.Id\r\njoin[Others].[Status]s on ta.VerifyStatus = s.id ";
			var results =await _conn.QueryAsync<TeacherApplyListDto>(sql);

            return results.ToList();
		}

        public async Task<List<TeacherResumeDto>> GetResumeList(string? search)
        {
            try
            {
                var sql = @"SELECT 
    t.Id AS TeacherId,
	m.Id AS MemberId,
    m.[Name] AS Name,
    m.ImageName,
    r.BankAccount AS BankAccount,
    r.Price AS Price,
    r.WorkExperience AS WorkExperience,
    m.Email AS Email,
    m.Phone AS Phone,
    (SELECT COUNT(mtr.Status) FROM Member.MembersTutorRecords mtr
        JOIN Teacher.TeachersRealTutorPeriods trtp ON mtr.TeacherTutorPeriodID = trtp.Id
        WHERE mtr.Status = 1 AND trtp.TeacherId = t.Id) AS FinishCount,
    (SELECT COUNT(mtr.Status) FROM Member.MembersTutorRecords mtr
        JOIN Teacher.TeachersRealTutorPeriods trtp ON mtr.TeacherTutorPeriodID = trtp.Id
        WHERE mtr.Status = 0 AND trtp.TeacherId = t.Id) AS UnfinishedCount,
    (SELECT AVG(c.Satisfaction) FROM Member.MembersTutorRecords mtr
        JOIN Teacher.TeachersRealTutorPeriods trtp ON mtr.TeacherTutorPeriodID = trtp.Id
        LEFT JOIN Comment.Comments c ON c.MemberTutorRecordId = mtr.Id
        WHERE trtp.TeacherId = t.Id) AS Satisfaction
FROM Member.Members m
JOIN Teacher.TeacherId t ON m.Id = t.MemberId
JOIN Teacher.TeachersResume r ON r.TeacherId = t.Id";

                var results = await _conn.QueryAsync<TeacherResumeDto>(sql);

                if (!string.IsNullOrEmpty(search))
                {
                    results = results.Where(x => x.Name.Contains(search) || x.Email.Contains(search));
                }

                return results.ToList();
            }
            catch (Exception)
            {
                throw new Exception("資料錯誤");
            }
        }

        public async Task<List<TeacherGetPriceDto>> GetPrice(int teacherId)
        {
            try
            {
                var sql = "select t.Id, tr.Price\r\nfrom[Member].[Members]m\r\njoin[Teacher].[TeacherId]t on m.Id =t.MemberId\r\njoin[Teacher].[TeachersResume] tr on t.Id = tr.TeacherId\r\nwhere t.Id=@teacherId";
                var results = await _conn.QueryAsync<TeacherGetPriceDto>(sql, new { TeacherId = teacherId });

                return results.ToList();
            }
            catch (Exception)
            {
                throw new Exception("資料錯誤");
            }
        }

        public async Task<List<TeacherTutorRecordDto>> GetTutorRecord(int teacherId)
        {
            try
            {
                var sql = @"select mt.Id, t.Id as TeacherId, tp.TutorStartTime, m.Name as MemberName, mm.Name as TeacherName, mt.Status as TutorStatus, case when mt.status = 0 THEN '未完成' when mt.status = 1 THEN '已完成' END AS StatusName, cc.Satisfaction, cc.CommentContent
from[Member].[MembersTutorRecords] mt
join[Member].[Members] m on mt.MemberId = m.Id
join[Teacher].[TeachersRealTutorPeriods] tp on mt.TeacherTutorPeriodID = tp.Id
left join[Comment].[Comments] cc on mt.Id = cc.MemberTutorRecordId
join[Teacher].[TeacherId] t on tp.TeacherId = t.Id
join Member.Members mm on t.MemberId = mm.Id";

                if (teacherId != 0)
                {
                    sql += " WHERE t.Id = @teacherId";
                }
                var results = await _conn.QueryAsync<TeacherTutorRecordDto>(sql, new { TeacherId = teacherId });
                return results.OrderByDescending(x=>x.TutorStartTime).ToList();
            }
            catch (Exception)
            {
                throw new Exception("資料錯誤");
            }
        }

		public async Task<List<TeacherCommentsDto>> GetComments(int id)
		{
			try
			{
				var sql = @"select mt.Id, m.Name as MemberName, oc.Name as CategoryName, cc.Satisfaction, cc.CommentContent 
                            from[Member].[MembersTutorRecords] mt 
                            join[Member].[Members] m 
                            on mt.MemberId = m.Id
                            join[Teacher].[TeachersRealTutorPeriods] tp 
                            on mt.TeacherTutorPeriodID = tp.Id
                            left join[Comment].[Comments] cc 
                            on mt.Id = cc.MemberTutorRecordId
                            join[Teacher].[TeacherId] t 
                            on tp.TeacherId = t.Id
                            join Member.Members mm on t.MemberId = mm.Id
                            join[Teacher].[TeachersApply] ta 
                            on t.Id = ta.TeacherId
                            join[Others].[Categories] oc 
                            on ta.CategoryId = oc.Id";

				if (id != 0)
				{
					sql += " WHERE mt.Id = @id";
				}
				var results = await _conn.QueryAsync<TeacherCommentsDto>(sql, new { Id = id });
				return results.ToList();
			}
			catch (Exception)
			{
				throw new Exception("資料錯誤");
			}
		}

        public async Task<List<TeacherCommentsDto>> BackGetComments(int id)
        {
            try
            {
                var sql = @"select cc.Id, m.Name as MemberName, cc.Satisfaction, cc.CommentContent
from[Member].[MembersTutorRecords] mt
join[Member].[Members] m on mt.MemberId = m.Id
join[Teacher].[TeachersRealTutorPeriods] tp on mt.TeacherTutorPeriodID = tp.Id
join[Comment].[Comments] cc on mt.Id = cc.MemberTutorRecordId
join[Teacher].[TeacherId] t on tp.TeacherId = t.Id
join Member.Members mm on t.MemberId = mm.Id";

                if (id != 0)
                {
                    sql += " WHERE t.Id = @id";
                }
                var results = await _conn.QueryAsync<TeacherCommentsDto>(sql, new { Id = id });
                return results.ToList();
            }
            catch (Exception)
            {
                throw new Exception("資料錯誤");
            }
        }

        public async Task<List<TeacherStudentDto>> GetStudentRecord(int teacherId)
        {
            try
            {
                var sql = @"SELECT m.Id as MemberId, m.Name as MemberName, m.ImageName as MemberImageName, m.Email, SUM(CASE WHEN mt.Status = 1 THEN 1 ELSE 0 END) as TrueTutor, SUM(CASE WHEN mt.Status = 0 THEN 1 ELSE 0 END) as FalseTutor, AVG(c.Satisfaction) as AvgSatisfaction
FROM [Member].[Members] m
JOIN [Member].[MembersTutorRecords] mt ON m.Id = mt.MemberId
JOIN [Teacher].[TeachersRealTutorPeriods] tt ON mt.TeacherTutorPeriodID = tt.Id
JOIN [Teacher].[TeacherId] t on tt.TeacherId =t.Id
LEFT JOIN [Comment].[Comments] c ON mt.Id = c.MemberTutorRecordId
Where t.Id=@teacherId 
GROUP BY m.Id, m.Name, m.Email, m.ImageName";
                var parameters = new { teacherId };
                var results = await _conn.QueryAsync<TeacherStudentDto>(sql, parameters);

                return results.ToList();
            }
            catch (Exception)
            {
                throw new Exception("資料錯誤");
            }
        }

    }
}
