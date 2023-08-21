using Microsoft.Data.SqlClient;
using Nsl_Core.Models.Dtos;
using NSL_html.Models.Builder;
using System.Data;
using System.Reflection;
using Nsl_Core.Models.Interfaces;
using Nsl_Core.Models.Dtos.Member.Manager;

namespace Nsl_Core.Models.Infra.Repositories.ADORepositories
{
    public class MemberRepository : IMemberRepo
    {
        public MemberDto Get(int? memberId)

        {
            if (memberId == null) return null;
            string sql = $"Select * From Member.Members Where Id ={memberId}";
            Func<SqlDataReader, MemberDto> func = Assembler.MemberDtoAssembler;
            SqlParameter[] parameters = new SqlParameter[0];
            Func<SqlConnection> connGetter = SqlDb.GetConnection;

            return SqlDb.Get(connGetter, sql, func);

        }

        public int Create(MemberDto dto)
        {
            string sql = @"insert into [Member].[Members]
                        ([Name],[Birthday],[Phone],[Email],[Password],[gender],
                        [CityId],[AreaId],[ImageName],[EmailCheck],[Role])
                        values(@Name,@Birthday,@Phone,@Email,@password,@gender,
                        @cityid,@areaid,@imagename,@emailcheck,@role)";

            var parameters = new SqlParameterBuilder()
                           .AddNVarChar("@Name", 20, dto.Name)
                           .AddNullableDateTIme("@Birthday", dto.Birthday)
                           .AddNVarChar("@Phone", 50, dto.Phone)
                           .AddNVarChar("@Email", 50, dto.Email)
                           .AddNVarChar("@Password", 50, dto.Password)
                           .AddNullableBool("@Gender", dto.Gender)
                           .AddNullableInt("@CityId", dto.CityId)
                           .AddNullableInt("@AreaId", dto.AreaId)
                           .AddNVarChar("@ImageName", 50, dto.ImageName)
                           .AddInt("@Role", dto.Role)
                           .Build();
            return SqlDb.Create(SqlDb.GetConnection, sql, parameters);
        }


        public int Update(MemberDto dto)
        {
            string sql = @" update [Member].[Members] set
                       
                       Name = @Name,Birthday = @Birthday,phone = @Phone,email = @Email,
                       password = @password,gender = @gender,
                       cityid= @cityid ,areaid = @Areaid,imagename = @imagename,
                       emailcheck = @emailcheck,role = @role ,modifiedTime = @ModifiedTime
                        where Id = @Id";

            var parameters = new SqlParameterBuilder()
                .AddInt("@Id", dto.Id)
                           .AddNVarChar("@Name", 20, dto.Name)
                           .AddNullableDateTIme("@Birthday", dto.Birthday)
                           .AddNVarChar("@Phone", 50, dto.Phone)
                           .AddNVarChar("@Email", 50, dto.Email)
                           .AddNVarChar("@Password", 50, dto.Password)
                           .AddNullableBool("@Gender", dto.Gender)
                           .AddNullableInt("@CityId", dto.CityId)
                           .AddNullableInt("@AreaId", dto.AreaId)
                           .AddNVarChar("@ImageName", 50, dto.ImageName)
                           .AddInt("@Role", dto.Role)
                           .AddNullableDateTIme("@ModifiedTime", dto.ModifiedTime)
                           .Build();

            return SqlDb.UpdateOrDelete(SqlDb.GetConnection, sql, parameters);
        }

        public MemberDto GetByEmail(string email)
        {
            string sql = $"Select*From Member.members Where Email=@Email";
            Func<SqlDataReader, MemberDto> func = Assembler.MemberDtoAssembler;
            SqlParameter parameter = new SqlParameter("@Email", SqlDbType.NVarChar, 30) { Value = email };
            Func<SqlConnection> connGetter = SqlDb.GetConnection;

            return SqlDb.Get(connGetter, sql, func, parameter);

        }

        public int Delete(int MemberId)
        {
            string sql = "DELETE FROM Member.members Where Id=@Id";

            SqlParameter parameter = new SqlParameter("@Id", SqlDbType.Int) { Value = MemberId };
            Func<SqlConnection> connGetter = SqlDb.GetConnection;

            return SqlDb.UpdateOrDelete(connGetter, sql, parameter);

        }

        public List<MemberConsumerRecordDto> GetList(int? memberId)
        {
            throw new NotImplementedException();
        }

        public List<MemberGetDetailDto> GetDetail(string? ordercode)
        {
            throw new NotImplementedException();
        }
    }
}
