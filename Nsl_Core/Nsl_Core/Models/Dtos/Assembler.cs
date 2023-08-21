using Microsoft.Data.SqlClient;
using Nsl_Core.Models.Dtos.Member.Manager;
using Nsl_Core.Models.Dtos.Teacher.TeacherApply;
using NSL_html.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nsl_Core.Models.Dtos
{
    public class Assembler
    {
        public static Func<SqlDataReader, MemberDto> MemberDtoAssembler
        {
            get
            {
                Func<SqlDataReader, MemberDto> func = (reader) =>
                {

                    int id = reader.GetInt("Id");
                    string name = reader.GetString("Name");
                    bool? gender = reader.GetNullableBool("gender");
                    string phone = reader.GetString("Phone");
                    DateTime? birthday = reader.GetNullableDateTime("Birthday");
                    string email = reader.GetString("Email");
                    int cityid = reader.GetInt("CityId");
                    int areaid = reader.GetInt("AreaId");
                    int role = reader.GetInt("Role");
                    bool emailcheck = reader.GetBool("EmailCheck");
                    string imagename = reader.GetString("ImageName");
                    DateTime? createtime = reader.GetNullableDateTime("CreatedTime");
                    DateTime? modifiedtime = reader.GetNullableDateTime("ModifiedTime");
                    string password = reader.GetString("Password");

                    return new MemberDto
                    {
                        Id = id,
                        Name = name,
                        Gender = gender,
                        Birthday = birthday,
                        Phone = phone,
                        Email = email,
                        CityId = cityid,
                        AreaId = areaid,
                        EmailCheck = emailcheck,
                        Role = role,
                        ImageName = imagename,
                        Password = password,
                        CreateTime = createtime,
                        ModifiedTime = modifiedtime,

                    };
                };
                return func;
            }
        }

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
