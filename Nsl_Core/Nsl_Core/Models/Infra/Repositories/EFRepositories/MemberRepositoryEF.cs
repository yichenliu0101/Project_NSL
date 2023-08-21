using Microsoft.EntityFrameworkCore;
using Nsl_Core.Models.Interfaces;
using Nsl_Core.Models.Infra.Repositories;
using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Infra.EntitiesTransfer;
using Nsl_Core.Models.Dtos.Member.Manager;
using System.Reflection;

namespace Nsl_Core.Models.Infra.Repositories.EFRepositories
{
    public class MemberRepositoryEF:IMemberRepo
    {
		private readonly NSL_DBContext _context;

		public MemberRepositoryEF(NSL_DBContext context)
		{
			_context = context;
		}
		public int Create(MemberDto dto)
        {
            var db = new NSL_DBContext();
            var member = dto.MemberDtoToMember();

            db.Members.Add(member);

            db.SaveChanges();

            return db.Members.ToList().Last().Id;

        }

        public MemberDto Get(int? memberId)
        {
            if (memberId == null) throw new Exception("資料庫問題");
            try
            {
                var dto = _context.Members.Where(x => x.Id == memberId)
                            .ToList()
                            .Select(x => new MemberDto
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Gender = Convert.ToBoolean(x.Gender),
                                Phone = x.Phone,
                                Password = x.Password,
                                Email = x.Email,
                                CityId = Convert.ToInt32(x.CityId),
                                AreaId = Convert.ToInt32(x.AreaId),
                                EmailCheck = Convert.ToBoolean(x.EmailCheck),
                                Role = x.Role,
                                ImageName = x.ImageName,
                                Birthday = x.Birthday
                            }).FirstOrDefault();
                return dto;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        public int Update(MemberDto dto)
        {
            var member = _context.Members.Find(dto.Id);
            string hashPassword = new HashPassword(_context).GenerateHashPassword(member.Password, out string salt);
            member.Name = dto.Name;
            member.Password = dto.Password;
            member.Salt = salt;
            member.EncryptedPassword = hashPassword;
            member.Email = dto.Email;
            member.Phone = dto.Phone;
            member.ImageName = dto.ImageName;
            member.Birthday = dto.Birthday;
            member.CityId = dto.CityId;
            member.AreaId = dto.AreaId;
            member.Gender = dto.Gender;
            

            _context.SaveChanges();

            return _context.Members.Count(x => x.Id == dto.Id);

        }

        public List<MemberConsumerRecordDto> GetList(int? memberId)
        {
            List<MemberConsumerRecordDto> result = new List<MemberConsumerRecordDto>();

            var query = (from mmc in _context.MembersConsumptionRecords
                         join mmcd in _context.MembersConsumptionRecordDetails on mmc.Id equals mmcd.MembersConsumptionRecordId
                         join op in _context.PaymentMethods on mmc.PaymentId equals op.Id
                         where mmc.MemberId == memberId
                         group new { mmc, mmcd, op } by new { mmc.Id, mmc.OrderCode, mmc.ConsumeTime, op.PaymentMethod } into g
                         select new MemberConsumerRecordDto
                         {
                             Id = g.Key.Id,
                             OrderCode = g.Key.OrderCode,
                             ConsumeTime = g.Key.ConsumeTime,
                             PaymentMethod = g.Key.PaymentMethod,
                             Total = g.Sum(x => x.mmcd.CurrentPrice * x.mmcd.Count),
                         }).ToList();
            return query;
        }

        public List<MemberGetDetailDto> GetDetail(string? ordercode)
        {

            var query = from mcrd in _context.MembersConsumptionRecordDetails
                        join mcr in _context.MembersConsumptionRecords on mcrd.MembersConsumptionRecordId equals mcr.Id
                        join tt in _context.TeacherId on mcrd.TeacherId equals tt.Id
                        join m in _context.Members on tt.MemberId equals m.Id
                        join tr in _context.TeachersResume on tt.Id equals tr.TeacherId
                        where mcr.OrderCode == ordercode

                        select new MemberGetDetailDto
                        {
                            ConsumeId = mcr.Id,
                            TeacherName = m.Name,
                            Title = tr.Title,
                            Price = tr.Price,
                            Count = mcrd.Count,
                        };

            return query.ToList();
        }

    }
}
