using Humanizer;
using Nsl_Core.Models.Dtos.Member.Login;
using Nsl_Core.Models.Dtos.Website.Index;
using Nsl_Core.Models.EFModels;
using Nsl_Core.Models.Infra.EntitiesTransfer;
using Nsl_Core.Models.ViewModels;
using NSL_html.Models.Infra;
using System.Diagnostics.Metrics;

namespace Nsl_Core.Models.Infra.Repositories.EFRepositories
{
    public class NSLRepositoriresEF
    {
        private readonly NSL_DBContext _db;

        public NSLRepositoriresEF(NSL_DBContext db)
        {
            _db = db;
            IndexDto = LoadIndexDto();
        }

        public IndexDto IndexDto { get; private set; }

        public IndexDto LoadIndexDto()
        {
            var indexDto = new IndexDto();
            indexDto.Member = _db.Members.Count(x=>x.Role != 3);
            indexDto.Teacher = _db.Members.Count(x => x.Role == 2);
            indexDto.FeedBack = _db.Comments.Count();

            indexDto.Comments = (from c in _db.Comments
                            join mtr in _db.MembersTutorRecords on c.MemberTutorRecordId equals mtr.Id
                            join trtp in _db.TeachersRealTutorPeriods on mtr.TeacherTutorPeriodId equals trtp.Id
                            join m in _db.Members on mtr.MemberId equals m.Id
                            join t in _db.TeacherId on trtp.TeacherId equals t.Id
                            join ta in _db.TeachersApply on t.Id equals ta.TeacherId
                            join ca in _db.Categories on ta.CategoryId equals ca.Id
                            join mm in _db.Members on t.MemberId equals mm.Id
                            select new IndexMemberCommentDto()
                            {
                                ImageName = mm.ImageName,
                                TeacherName = mm.Name,
                                MemberEmail = m.Email,
                                Category = ca.Name,
                                Satisfaction = (int)c.Satisfaction,
                                CommentContent = c.CommentContent
                            }).OrderByDescending(x => x.Satisfaction).ThenByDescending(x => x.CommentContent.Length).Take(5).ToList();

            indexDto.Articles = _db.Articles.OrderByDescending(x => x.CreatedTime).Take(3).ToList();
            return indexDto;
        }

        public LoginDto Register(RegisterVM vm)
        {
            var memberInDb = _db.Members.FirstOrDefault(x=>x.Email==vm.Email);
            if (memberInDb != null) throw new Exception("註冊失敗，此帳號已經是會員");
            var member = vm.RegisterVMToMember(_db);        

            _db.Add(member);
            _db.SaveChanges();
            EmailHelper.VerifyMember(member);

            return new LoginDto() { Id = member.Id, Name = member.Name, ImageName = member.ImageName, Role = 1};
        }

        public LineLoginDto LineRegister(LineRegisterVM vm)
        {
            var memberInDb = _db.Members.FirstOrDefault(x => x.LineId == vm.LineId);
            if (memberInDb != null) throw new Exception("註冊失敗，此帳號已經是會員");
            var member = vm.LineRegisterVMToMember(_db);

            _db.Add(member);
            _db.SaveChanges();

            return new LineLoginDto() { Id = member.Id, Name = member.Name, ImageName = member.ImageName, Role = 1, EmailCheck = true, LineId = member.LineId};
        }
        public string ForgetPassword(ForgetPasswordVM vm)
        {

            var memberInDb = _db.Members.FirstOrDefault(x => x.Email == vm.Email);
            if (memberInDb == null) throw new Exception("查無此資料");

            var newEmailToken = Guid.NewGuid().ToString();
            memberInDb.EmailToken = newEmailToken;
            _db.SaveChanges();

            var member = vm.ForgetPasswordVMToMember(_db);

            EmailHelper.ForgetPassword(member);
            return member.Email;
        }
    }
}
