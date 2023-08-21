using Microsoft.Data.SqlClient;
using Nsl_Core.Models.Dtos;
using Nsl_Core.Models.Dtos.Teacher.TeacherApply;
using Nsl_Core.Models.Interfaces;
using NuGet.DependencyResolver;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace Nsl_Core.Models.Infra.Repositories.ADORepositories
{
    public class TeacherRepository 
    {
        public TeacherApplyListDto GetApplyList(int teacherId)
        {
            string sql = $"select m.Name as Name, c.Name as CategoryName, s.name as StatusName " +
                         $"from[Teacher].[TeacherId]tid " +
                         $"join[Member].[Members] m on tid.MemberId = m.Id " +
                         $"join[Teacher].[TeachersApply]ta on tid.Id = ta.TeacherId " +
                         $"join[Others].[Categories]c on ta.CategoryId = c.Id " +
                         $"join[Others].[Status]s on ta.VerifyStatus = s.id " +
                         $"where tid.id ={teacherId}";
            Func<SqlDataReader, TeacherApplyListDto> func = Assembler.TeacherApplyListDtoAssembler;//sAssembler裡的dto
            SqlParameter[] parameters = new SqlParameter[0];//替換@name那邊照抄
            Func<SqlConnection> connGetter = SqlDb.GetConnection;//SqlConnection連線字串,反正就照用

            return SqlDb.Get(connGetter, sql, func);//取出你指定的那個id的dto資料
        }
    }
}
