using Microsoft.EntityFrameworkCore;
using Nsl_Core.Models.Dtos.Member.Manager;
using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Interfaces;
using System.Diagnostics.Metrics;

namespace Nsl_Core.Models.Infra.Repositories.EFRepositories
{
    public class MemberConsumeRecordEF : IMemberConsumeRecodRepo
    {
        private readonly NSL_DBContext _context;

        public MemberConsumeRecordEF(NSL_DBContext context)
        {
            _context = context;
        }


        public List<MemberConsumerRecordDto> GetList(int? memberId)
        {
            List<MemberConsumerRecordDto> result = new List<MemberConsumerRecordDto>();

            var query = (from mmc in _context.MembersConsumptionRecords
                        join mmcd in _context.MembersConsumptionRecordDetails on mmc.Id equals mmcd.MembersConsumptionRecordId
                        join op in _context.PaymentMethods on mmc.PaymentId equals op.Id
                        where mmc.MemberId == memberId
                        group new { mmc, mmcd, op } by new {mmc.Id, mmc.OrderCode, mmc.ConsumeTime, op.PaymentMethod } into g
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

