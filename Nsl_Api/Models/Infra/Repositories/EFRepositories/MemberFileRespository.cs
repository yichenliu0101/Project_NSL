using Microsoft.EntityFrameworkCore;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Interfaces;
using Nsl_Api.Models.Dtos;

namespace Nsl_Api.Models.Infra.Repositories.EFRepositories
{
    public class MemberFileRespository : IMemberFileRepo
    {
        private NSL_DBContext _db;

        public MemberFileRespository(NSL_DBContext db)
        {
            _db = db;
        }
        public Task<List<MemberFileDto>> GetMemberFile(int memberId)
        {
            #region Query
            var query = from m in _db.Members
                        join c in _db.Citys
                             on m.CityId equals c.Id into jt1
                        from jis1 in jt1.DefaultIfEmpty()
                        join a in _db.Areas
                             on m.AreaId equals a.Id into jt2
                        from jis2 in jt2.DefaultIfEmpty()
                        where m.Id == memberId
                        select new MemberFileDto
                        {
                            MemberId = m.Id,
                            Name = m.Name,
                            Gender = (m.Gender == null) ? "" : ((bool)m.Gender ? "男" : "女"),
                            Birthday = m.Birthday,
                            Email = (m.Email == null) ? "" : m.Email,
                            CityName = (jis1.Name == null) ? "" : jis1.Name,
                            AreaName = (jis2.Name == null) ? "" : jis2.Name,
                            ImageName = (m.ImageName == null) ? "default.jpg" : m.ImageName,
                            Password = m.Password,
                        };

            #endregion
           
            return query.AsNoTracking().ToListAsync();
        }

        public Task<List<MemberDetailDto>>GetMemberDetail(int? consumeId) 
        {
            var query = from mcrd in _db.MembersConsumptionRecordDetails
                        join mcr in _db.MembersConsumptionRecords on mcrd.MembersConsumptionRecordId equals mcr.Id
                        join tt in _db.TeacherId on mcrd.TeacherId equals tt.Id
                        join m in _db.Members on tt.MemberId equals m.Id
                        join tr in _db.TeachersResume on tt.Id equals tr.TeacherId
                        where mcr.Id == consumeId
                        select new MemberDetailDto
                        {
                            ConsumeId = mcr.Id,
                            TeacherName = m.Name,
                            Title = tr.Title,
                            Price = tr.Price,
                            Count = mcrd.Count,
                        };

            return query.AsNoTracking().ToListAsync(); 

        }




    }
}
