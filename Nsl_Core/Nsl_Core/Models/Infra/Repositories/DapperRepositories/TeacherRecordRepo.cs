using Dapper;
using Microsoft.Data.SqlClient;
using Nsl_Core.Models.Dtos.Teacher.TeacherRecord;
using System.Linq;
using System.Transactions;

namespace Nsl_Core.Models.Infra.Repositories.DapperRepositories
{
    public class TeacherRecordRepo
    {
        private readonly SqlConnection _conn;
        public TeacherRecordRepo(IConfiguration configuration)
        {
            _conn = new SqlConnection(configuration.GetConnectionString("NSLDbContext"));
        }
        public TeacherRecordDto GetRecord(int id)
        {
            var dto = new TeacherRecordDto();
            using (var tranScope = new TransactionScope())
            {


                using (_conn)
                {
                    string sqlTeacherData = @"SELECT t.Id AS Id, m.[Name] AS Name,r.BankAccount AS BankAccount,r.Price AS Price,r.WorkExperience AS WorkExperience,m.Email AS Email,m.Phone AS Phone
                                                FROM Member.Members m
                                                JOIN Teacher.TeacherId t ON m.Id=t.MemberId
                                                JOIN Teacher.TeachersResume r ON r.TeacherId=t.Id
                                                WHERE t.id=@Id";
                    var resultTData = _conn.Query<TeacherData>(sqlTeacherData, new { Id= id }).ToList().FirstOrDefault();

                    string sqlFinishedCount = @"SELECT COUNT(mtr.Status) AS FinishCount
                                                FROM Member.Members m
                                                JOIN Teacher.TeacherId t ON m.Id=t.MemberId
                                                JOIN Teacher.TeachersRealTutorPeriods trtp ON t.Id=trtp.Id
                                                JOIN Member.MembersTutorRecords mtr
                                                ON mtr.TeacherTutorPeriodID=trtp.Id
                                                WHERE mtr.status = 1 and t.id = @Id";
                    var resultsFinishCount = _conn.Query<StatusCount>(sqlFinishedCount, new { Id = id }).ToList().FirstOrDefault();

                    string sqlUnfinishedCount = @"SELECT COUNT(mtr.Status)
                                                    FROM Member.Members m
                                                    JOIN Teacher.TeacherId t
                                                    ON m.Id=t.MemberId
                                                    JOIN Teacher.TeachersRealTutorPeriods trtp
                                                    ON t.Id=trtp.Id
                                                    JOIN Member.MembersTutorRecords mtr
                                                    ON mtr.TeacherTutorPeriodID=trtp.Id
                                                    WHERE mtr.status = 0 AND t.id =@Id";
                    var resultsUnFinishCount = _conn.Query<StatusCount>(sqlUnfinishedCount, new { Id = id }).ToList().FirstOrDefault();

                    string sqlSatisfaction = @"SELECT AVG(c.Satisfaction)
                                                FROM Member.Members m
                                                JOIN Teacher.TeacherId t
                                                ON m.Id=t.MemberId
                                                JOIN Teacher.TeachersRealTutorPeriods trtp
                                                ON t.Id=trtp.Id
                                                JOIN Member.MembersTutorRecords mtr
                                                ON mtr.TeacherTutorPeriodID=trtp.Id
                                                JOIN Comment.Comments c
                                                ON c.MemberTutorRecordId=mtr.Id
                                                WHERE t.Id=@Id";
                    var resultsSatisfaction = _conn.Query<Satisfaction>(sqlSatisfaction, new { Id = id }).ToList().FirstOrDefault();

                    dto.Name = resultTData.Name;
                    dto.BankAccount = resultTData.BankAccount;
                    dto.Price = resultTData.Price;
                    dto.WorkExperience = resultTData.WorkExperience;
                    dto.Email = resultTData.Email;
                    dto.Phone = resultTData.Phone;
                    dto.FinishCount = resultsFinishCount.Status;
                    dto.UnfinishedCount = resultsUnFinishCount.Status;
                    dto.Satisfaction = resultsSatisfaction.Star;
                }
                tranScope.Complete();
            }
            return dto;
        }

        public List<TeacherCommentsDto> GetComments(int id)
        {
            string sql = @"SELECT t.Id AS Id,sm.Name AS Name,c.CommentContent AS CommentContent,c.Satisfaction AS Satisfaction
                            FROM Member.Members m
                            JOIN Teacher.TeacherId t ON m.Id=t.MemberId
                            JOIN Teacher.TeachersRealTutorPeriods trtp ON t.Id=trtp.Id
                            JOIN Member.MembersTutorRecords mtr ON mtr.TeacherTutorPeriodID=trtp.Id
                            JOIN Comment.Comments c ON c.MemberTutorRecordId=mtr.Id
                            JOIN Member.Members sm ON sm.Id=mtr.MemberId
                            WHERE t.Id=@id";
            var dto = _conn.Query<TeacherCommentsDto>(sql, new { Id = id }).ToList();

            return dto;
        }
    }
}
