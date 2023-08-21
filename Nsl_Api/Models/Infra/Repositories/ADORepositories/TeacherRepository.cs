using Microsoft.Data.SqlClient;
using Nsl_Api.Models.Dtos;
using Nsl_Api.Models.Interfaces;
using NSL_html.Models;
using System.Reflection;

namespace Nsl_Api.Models.Infra.Repositories.ADORepositories
{
    public class TeacherRepository 
    {
        //public TeacherApplyListDto GetApplyList(int teacherId)
        //{
        //    string sql = $"select m.Name as TeacherName, c.Name as CategoryName, s.name as StatusName " +
        //                 $"from[Teacher].[TeacherId]tid " +
        //                 $"join[Member].[Members] m on tid.MemberId = m.Id " +
        //                 $"join[Teacher].[TeachersApply]ta on tid.Id = ta.TeacherId " +
        //                 $"join[Others].[Categories]c on ta.CategoryId = c.Id " +
        //                 $"join[Others].[Status]s on ta.VerifyStatus = s.id " +
        //                 $"where tid.id ={teacherId}";
        //    Func<SqlDataReader, TeacherApplyListDto> func = Assembler.TeacherApplyListDtoAssembler;
        //    SqlParameter[] parameters = new SqlParameter[0];
        //    Func<SqlConnection> connGetter = SqlDb.GetConnection;

        //    return SqlDb.Get(connGetter, sql, func);
        //}
    }
}
