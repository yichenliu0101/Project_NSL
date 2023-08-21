using Nsl_Core.Models.Dtos.Member.Manager;
using Nsl_Core.Models.EFModels;
using System.Linq;

namespace Nsl_Core.Models.Infra.Repositories.EFRepositories
{
    public class MemberCommentEFRepo
    {
        private readonly NSL_DBContext _db;
        public MemberCommentEFRepo(NSL_DBContext db)
        {
            _db = db;
        }

        public string EditOrCreate(int memberTutorPeriodId, string commentContent, double satisfaction)
        {
            var inDb=_db.Comments.Where(x=>x.MemberTutorRecordId== memberTutorPeriodId).FirstOrDefault();
            if (inDb == null)
            {
                var model = new Comments()
                {
                    CommentContent = commentContent,
                    MemberTutorRecordId = memberTutorPeriodId,
                    Satisfaction = satisfaction,
                    ModifiedTime = DateTime.Now,
                    CreatedTime = DateTime.Now
                };
                _db.Comments.Add(model);
                _db.SaveChanges();
                return "新增成功";
            }
            else
            {
                inDb.ModifiedTime = DateTime.Now;
                inDb.MemberTutorRecordId= memberTutorPeriodId;
                inDb.CommentContent = commentContent;
                inDb.Satisfaction = satisfaction;
                _db.SaveChanges();
                return "修改成功";
            }
        }
    }
}
