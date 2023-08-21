using Dapper;
using Microsoft.Data.SqlClient;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Dtos;
using System.Drawing;

namespace Nsl_Api.Models.Infra.Repositories.DapperRepositories

{
    public class ShoppingCarListDappers
    {
        private readonly SqlConnection _conn;
        public ShoppingCarListDappers(IConfiguration configuration)
        {
            _conn = new SqlConnection(configuration.GetConnectionString("NSLDbContext"));
        }

        public async Task<List<GetAllShopListDto>> GetAllShopList(string ordercode)
        {
            try
            {
                var sql = "select mcr.OrderCode,m.Name as TeacherName,tr.Title,mcrd.Count,mcrd.CurrentPrice,c.Name as CouponName,isnull(c.DiscountMoney,0) as DiscountMoney,\r\npm.PaymentMethod as PaymentName,mcr.ConsumeTime,(mcrd.count*mcrd.CurrentPrice) as AllPrice\r\nfrom[Member].[MembersConsumptionRecordDetails]mcrd\r\njoin[Member].[MembersConsumptionRecords]mcr\r\non mcr.Id = mcrd.MembersConsumptionRecordId\r\njoin [Teacher].[TeachersResume]tr\r\non mcrd.TeacherId = tr.TeacherId\r\njoin[Teacher].[TeacherId]t\r\non mcrd.TeacherId = t.Id\r\njoin[Member].[Members]m\r\non m.Id = t.MemberId\r\nleft join[Coupon].[Coupons]c\r\non c.Id = mcr.CouponId\r\njoin[Others].[PaymentMethods]pm\r\non pm.Id = mcr.PaymentId\r\nwhere mcr.OrderCode = @ordercode";
                var results = await _conn.QueryAsync<GetAllShopListDto>(sql, new { OrderCode = ordercode });

                return results.ToList();
            }
            catch (Exception)
            {
                throw new Exception("資料錯誤");
            }

        }
    }
}
