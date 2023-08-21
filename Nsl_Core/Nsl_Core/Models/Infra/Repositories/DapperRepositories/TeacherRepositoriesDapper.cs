using Dapper;
using Microsoft.Data.SqlClient;
using Nsl_Core.Models.Dtos.Teacher.TeacherApply;
using Nsl_Core.Models.Dtos.Teacher.TeacherResume;

namespace Nsl_Core.Models.Infra.Repositories.DapperRepositories
{
    public class TeacherRepositoriesDapper
	{
        private readonly SqlConnection _conn;

        public TeacherRepositoriesDapper(IConfiguration configuration)
        {
            _conn = new SqlConnection(configuration.GetConnectionString("NSLDbContext"));
        }
		public TeacherApplyDto GetApply(int id)
		{
			try
			{
				var sql = "select ta.TeacherId as Id,m.Name as TeacherName, c.Name as CategoryName, m.ImageName, STRING_AGG(ol.Name,', ') as LanguageName, ot.Name as TutorExperienceName, ow.Name as WorkStatusName, oth.Name as TutorHoursOfWeekName, ort.Name as RevenueTargetName, ta.Intro\r\nfrom[Teacher].[TeacherId] tid\r\njoin[Member].[Members] m on tid.MemberId = m.Id\r\njoin[Teacher].[TeachersApply] ta on tid.Id = ta.TeacherId\r\njoin[Others].[Categories] c on ta.CategoryId = c.Id \r\nleft join [Teacher].[TeachersLanguages] ttl on ta.TeacherId = ttl.TeacherId\r\nleft join [Others].[Languages] ol on ttl.LanguageId= ol.Id\r\njoin[Others].[TutorExperience] ot on ta.TutorExperienceId = ot.Id\r\njoin[Others].[WorkStatus] ow on ta.WorkStatusId = ow.Id\r\njoin[Others].[TutorHoursOfWeek]oth on ta.TutorHoursOfWeekId = oth.Id\r\njoin[Others].[RevenueTarget] ort on ta.RevenueTargetId = ort.Id\r\nwhere ta.teacherId =@id\r\ngroup by ta.TeacherId, m.Name,c.Name, m.ImageName, ot.Name, ow.Name, oth.Name, ort.Name, ta.Intro";
				var dto = _conn.Query<TeacherApplyDto>(sql, new { Id = id });

				return dto.FirstOrDefault();
			}
			catch(Exception)
			{
				throw new Exception("此教師申請資料不齊全");
			} 
		}

        public TeacherResumeDto GetResume(int memberId)
        {
            try
            {
                var sql = "select t.Id as TeacherId, m.Id as MemberId, m.Name as TeacherName, m.ImageName, b.Name as BankCodeName, ts.BankAccount, ts.Education, ts.WorkExperience, ts.Title, ts.Introduction\r\nfrom[Teacher].[TeachersResume]ts\r\njoin[Teacher].[TeacherId]t on ts.TeacherId=t.Id\r\njoin[Member].[Members]m on t.MemberId=m.Id\r\nleft join[Others].[BankCode]b on ts.BankCodeId = b.Id\r\nwhere m.Id=@memberId";
                var dto = _conn.Query<TeacherResumeDto>(sql, new { MemberId = memberId });

                return dto.FirstOrDefault();
            }
            catch (Exception)
            {
                throw new Exception("資料錯誤");
            }
        }

        public TeacherTutorRecordDto GetTeacherTutorRecord(int memberId)
        {
            try
            {
                var sql = "select m.Id as MemberId,t.Id as TeacherId, m.name as TeacherName, m.ImageName, c.Name as CategoryName\r\nfrom[Member].[Members]m\r\njoin[Teacher].[TeacherId]t on m.Id = t.MemberId\r\njoin[Teacher].[TeachersApply]ta on t.Id =ta.TeacherId\r\njoin[Others].[Categories]c on ta.CategoryId =c.Id\r\nwhere m.Id = @memberId";
                var dto = _conn.Query<TeacherTutorRecordDto>(sql, new { MemberId = memberId });

                return dto.FirstOrDefault();
            }
            catch (Exception)
            {
                throw new Exception("資料錯誤");
            }
        }

        public TeacherEditDto GetResumeEdit(int memberId)
        {
            try
            {
                var sql = "select ts.teacherId as TeacherId, m.Id as MemberId, b.Id as BankCodeId,  b.Name as BankCodeName, ts.BankAccount, ts.Education, ts.WorkExperience, ts.Title, ts.Introduction\r\nfrom[Teacher].[TeachersResume]ts\r\njoin[Teacher].[TeacherId]t on ts.TeacherId=t.Id\r\njoin[Member].[Members]m on t.MemberId=m.Id\r\nleft join[Others].[BankCode]b on ts.BankCodeId = b.Id\r\nwhere m.Id=@memberId";
                var dto = _conn.Query<TeacherEditDto>(sql, new { MemberId = memberId });

                return dto.FirstOrDefault();
            }
            catch (Exception)
            {
                throw new Exception("資料錯誤");
            }
        }

        public BackTeacherResumeDto GetBackTeacherResume(int teacherId)
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
	b.Name AS BankCodeName,
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
JOIN Teacher.TeachersResume r ON r.TeacherId = t.Id
LEFT JOIN [Others].[BankCode] b ON r.BankCodeId = b.Id
WHERE t.Id = @teacherId";
                var dto = _conn.Query<BackTeacherResumeDto>(sql, new { TeacherId = teacherId });

                return dto.FirstOrDefault();
            }
            catch (Exception)
            {
                throw new Exception("資料錯誤");
            }
        }
    }
}
