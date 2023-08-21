using Microsoft.Data.SqlClient;
using NSL_html.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nsl_Api.Models.Dtos
{
    public class Assembler
    {

        public static Func<SqlDataReader, TeacherApplyListDto> TeacherApplyListDtoAssembler
        {
            get
            {
                Func<SqlDataReader, TeacherApplyListDto> func = (reader) =>
                {
                    string teacherName = reader.GetString("teachername");
                    string categoryName = reader.GetString("categoryname");
                    string statusName = reader.GetString("statusname");
                    return new TeacherApplyListDto
                    {
                        TeacherName = teacherName,
                        CategoryName = categoryName,
                        StatusName = statusName,
                    };
                };
                return func;
            }
        }

    }
}
