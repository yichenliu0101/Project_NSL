using Dapper;
using MessagePack;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Nsl_Api.Models.DTOs;

namespace Nsl_Api.Models.Infra.Repositories.DapperRepositories
{
	public class TeacherInfoRepo
	{
		private readonly SqlConnection _conn;
		public TeacherInfoRepo(IConfiguration configuration)
		{
			_conn = new SqlConnection(configuration.GetConnectionString("NSLDbContext"));
		}
		public async Task<TeacherInfoDto> GetResume(int teacherId)
		{
			var sql = @"SELECT TOP(1) t.Id AS TeacherId,m.Name AS TeacherName,tr.Title AS Title,tr.Introduction AS Introduction,cc.Satisfaction AS Satisfaction,
						cc.CommentContent AS CommentContent,m.ImageName AS TeacherImage,m2.ImageName AS MemberImage,tr.WorkExperience AS WorkExperience,tr.Education AS Education
                        FROM Member.Members m
                        JOIN Teacher.TeacherId t ON m.Id=t.MemberId
                        LEFT JOIN Teacher.TeachersApply ta ON ta.TeacherId=t.Id
                        LEFT JOIN Teacher.TeachersRealTutorPeriods trtp ON trtp.TeacherId=t.Id
                        LEFT JOIN Member.MembersTutorRecords mtr ON mtr.TeacherTutorPeriodID=trtp.Id
                        LEFT JOIN Comment.Comments cc ON cc.MemberTutorRecordId=mtr.Id
                        LEFT JOIN Teacher.TeachersResume tr ON tr.TeacherId=t.id
                        LEFT JOIN Member.Members m2 ON m2.Id=mtr.MemberId
                        WHERE t.id=@Id
                        ORDER BY cc.CreatedTime DESC";
			var results = await _conn.QueryAsync<TeacherInfoDto>(sql, new { Id = teacherId });

			return results.FirstOrDefault();
		}

		public async Task<List<TagDto>> GetTag(int teacherId)
		{
			var sql = @"SELECT ot.Name AS TagName
						FROM Member.Members m
						JOIN Teacher.TeacherId t ON t.MemberId=m.Id
						JOIN Teacher.TeachersTags tt ON tt.TeacherId=t.Id
						JOIN Others.Tags ot ON ot.Id=tt.TagId
						WHERE t.Id=@Id";
			var results = await _conn.QueryAsync<TagDto>(sql, new { Id = teacherId });

			return results.ToList();
		}
	}
}
