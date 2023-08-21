using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Infra.DataTransfer;
using Nsl_Api.Models.ViewModels;

namespace Nsl_Api.Models.Infra.Repositories.EFRepositories
{
	public class FrontTeacherRepositoriesEF
	{
		private readonly NSL_DBContext _db;
		public FrontTeacherRepositoriesEF(NSL_DBContext db) 
		{
			_db = db;
		}

		public string CreateTeacherApplyTransaction(TeacherApplyVM vm)
		{
			using(var transaction = _db.Database.BeginTransaction())
			{
				try
				{
					int teacherId = CreateTeacherId(vm.MemberId);
					CreateTeacherApply(vm, teacherId);
					CreateTeacherLanguages(vm, teacherId);

					transaction.Commit();

					return "Success";
				}
				catch(Exception ex)
				{
					transaction.Rollback();

					throw new Exception($"{ex.Message}");
				}
			}
		}

		private int CreateTeacherId(int? memberId)
		{
			if (memberId == null) throw new Exception("尚未登入，請先登入再填寫申請資料");
			var newTeacher = ((int)memberId).MemberIdToTeacherId();
			_db.TeacherId.Add(newTeacher);
			_db.SaveChanges();

			return newTeacher.Id;
		}

		private void CreateTeacherApply(TeacherApplyVM vm, int teacherId)
		{
			var teacherApply = vm.ToEntity();
			teacherApply.TeacherId = teacherId;

			_db.TeachersApply.Add(teacherApply);
			_db.SaveChanges();
		}

		private void CreateTeacherLanguages(TeacherApplyVM vm, int teacherId)
		{
			var teacherLanguages = vm.Language;
			foreach (var language in teacherLanguages)
			{
				var teacherLanguage = new TeachersLanguages();
				teacherLanguage.TeacherId = teacherId;
				teacherLanguage.LanguageId = language;
				_db.TeachersLanguages.Add(teacherLanguage);
				_db.SaveChanges();
			}
		}

		public bool GetTeacherId(int? memberId)
		{
			var memberIsApplied = _db.TeacherId.Where(x => x.MemberId == memberId).FirstOrDefault();
			var memberCurrentRole = _db.Members.Find(memberId);
			if (memberIsApplied != null) {
				if(memberCurrentRole != null&&memberCurrentRole.Role == 2)
				{
					throw new Exception("您已經是老師了！");
				}
                throw new Exception("您已申請過資料，還請耐心等候回覆，謝謝！");
            }
			return false;
		}
	}
}
