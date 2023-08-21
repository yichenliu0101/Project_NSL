using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Infra.Repositories.EFRepositories;
using Nsl_Api.Models.Interfaces;
using Nsl_Api.Models.ViewModels;

namespace Nsl_Api.Models.Service
{
	public class TutorService
	{
		private readonly NSL_DBContext _db;
		private readonly ITutorRepo _repo;
		public TutorService(NSL_DBContext db, ITutorRepo repo) 
		{
			_db = db;
			_repo = repo;
		}

		public void CreateNewCourse(TeachersRealTutorPeriods entity)
		{
			var recordInDb = _db.TeachersRealTutorPeriods
				.Where(x => x.TeacherId == entity.TeacherId && x.TutorStartTime == entity.TutorStartTime)
				.ToList();

			if (recordInDb.Count > 0) throw new Exception("您已經建立這個時段了！");

			_repo.CreateNewCourse(entity);
		}

		public string MemberSelectCourse(MemberSelectCourseVM vm)
		{
			//確認是否有相同時段的資料
			var periodInDb = (from mtr in _db.MembersTutorRecords
							 join trtp in _db.TeachersRealTutorPeriods on mtr.TeacherTutorPeriodId equals trtp.Id
							 where mtr.MemberId == vm.MemberId&&trtp.TutorStartTime == vm.TutorTime
							 select mtr).FirstOrDefault();

			if (periodInDb != null) throw new Exception("您在同一個時段已有其他預約的老師！");

			return _repo.MemberSelectCourse(vm);
		}
	}
}
