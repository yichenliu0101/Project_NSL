using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Infra.DataTransfer;
using Nsl_Api.Models.Interfaces;
using Nsl_Api.Models.ViewModels;
using NuGet.Protocol;
using System.Linq;
using static Dapper.SqlMapper;

namespace Nsl_Api.Models.Infra.Repositories.EFRepositories
{
    public class TutorRepositoriesEF : ITutorRepo
    {
        private NSL_DBContext _db;

        public TutorRepositoriesEF(NSL_DBContext db)
        {
            _db = db;
        }
        public Task<List<TutorPeriodFullCalendarDto>> GetTutorPeriodData(int teacherId)
        {
            var data = from ttp in _db.TeachersRealTutorPeriods
                       join t in _db.TeacherId on ttp.TeacherId equals t.Id
                       join tr in _db.TeachersResume on t.Id equals tr.TeacherId into tpt
                       from tptd in tpt.DefaultIfEmpty()
                       where t.Id == teacherId && ttp.TutorStartTime > DateTime.Now
                       select new TutorPeriodFullCalendarDto
                       {
                           Title = (ttp.Status) ? "已預約" : "未預約",
                           Start = ttp.TutorStartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                           End = ttp.TutorStartTime.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss"),
                           Color = (ttp.Status) ? "grey" : "orange"
                       };
            return data.ToListAsync();
        }

        public Task<List<TutorPeriodFullCalendarDto>> GetMemberTutorPeriodRecord(int memberId)
        {
            var data = from mtr in _db.MembersTutorRecords
                       join trtp in _db.TeachersRealTutorPeriods on mtr.TeacherTutorPeriodId equals trtp.Id
                       join t in _db.TeacherId on trtp.TeacherId equals t.Id
                       join m in _db.Members on t.MemberId equals m.Id
                       where mtr.MemberId == memberId 
                       //&& trtp.TutorStartTime > DateTime.Now
                       select new TutorPeriodFullCalendarDto
                       {
                           Title = m.Name,
                           Start = trtp.TutorStartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                           End = trtp.TutorStartTime.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss"),
                           Color = (mtr.Status) ? "grey" : "orange"
                       };
            return data.ToListAsync();
        }

        public Task<List<TutorPeriodFullCalendarDto>> GetTeacherTutorPeriodData(int teacherId)
        {
            var data = from trtp in _db.TeachersRealTutorPeriods
                       join mtr in _db.MembersTutorRecords on trtp.Id equals mtr.TeacherTutorPeriodId into mtrj
                       from mtri in mtrj.DefaultIfEmpty()
                       join m in _db.Members on mtri.MemberId equals m.Id into mj
                       from mi in mj.DefaultIfEmpty()
                       where trtp.TeacherId == teacherId && trtp.TutorStartTime > DateTime.Now
                       orderby trtp.TutorStartTime
                       select new TutorPeriodFullCalendarDto
                       {
                           Title = (string.IsNullOrEmpty(mi.Name)) ? "未預約" : mi.Name,
                           Start = trtp.TutorStartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                           End = trtp.TutorStartTime.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss"),
                           Color = (trtp.Status) ? "orange" : "grey"
                       };

            return data.ToListAsync();
        }

        public string CreateNewCourse(TeachersRealTutorPeriods entity)
        {
            try
            {
                _db.TeachersRealTutorPeriods.Add(entity);
                _db.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string DeleteCourse(TeacherDeleteCourseVM vm)
        {
            try
            {
                var entity = _db.TeachersRealTutorPeriods
                            .Where(x => x.TeacherId == vm.TeacherId && x.TutorStartTime == vm.StartTime)
                            .FirstOrDefault();

                if (entity == null) throw new Exception("找不到此資料");

                _db.TeachersRealTutorPeriods.Remove(entity);
                _db.SaveChanges();

                return "Success";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string EditDefaultTutorPeriod(TeacherDefaultPeriodVM vm)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    foreach (KeyValuePair<int, List<int>> kvp in vm.WeekPeriod)
                    {
                        //載入同星期同老師的舊資料
                        var dbDefaultTutor = _db.TeachersDefaultTutorPeriods
                                            .Where(x => x.TeacherId == vm.TeacherId && x.WeekdayId == kvp.Key)
                                            .Select(x => x.PeriodId)
                                            .ToList();

                        //加入沒有交集的新資料
                        CreateDefaultTutorPeriod(vm, dbDefaultTutor, kvp);

                        //刪除差集後的舊資料
                        DeleteDefaultTutorPeriod(vm, dbDefaultTutor, kvp);

                    }
                    //建立真實的教課資料
                    EditRealTutorPeriod(vm);

                    transaction.Commit();
                    return "Success";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }

            }
        }

        private void EditRealTutorPeriod(TeacherDefaultPeriodVM vm)
        {
            //找到所有已創建且並未預約的課程
            var dbRealTutor = _db.TeachersRealTutorPeriods
                                .Where(x => x.TeacherId == vm.TeacherId && x.Status == false && x.TutorStartTime > DateTime.Today)
                                .Select(x => x.TutorStartTime).ToList();
            //得到新的預設課程表
            var dbDefaultTutor = _db.TeachersDefaultTutorPeriods.Where(x => x.TeacherId == vm.TeacherId).ToList();

            var startDate = DateTime.Now.Date;

            //新增的真實課程資料
            List<DateTime> newList = new();

            for (int day = 0; day < 120; day++)
            {
                foreach (var defaultPeriod in dbDefaultTutor)
                {
                    int currentWeek = (int)startDate.DayOfWeek;
                    if (currentWeek == 0) currentWeek = 7;
                    if (defaultPeriod.WeekdayId == currentWeek)
                    {
                        //製作上課時間
                        DateTime realTutorPeriod = startDate;
                        realTutorPeriod = startDate.AddHours(defaultPeriod.PeriodId - 1);
                        //如果是與已創建資料沒有交集的新課程就創建新資料
                        if (!dbRealTutor.Contains(realTutorPeriod))
                        {
                            CreateRealTutorPeriod(vm, realTutorPeriod);
                        }
                        newList.Add(realTutorPeriod);
                    }
                }
                startDate = startDate.AddDays(1);
            }

            //如果是與新資料有差集的則刪除
            DeleteRealTutorPeriod(vm, dbRealTutor, newList);
        }

        private void CreateRealTutorPeriod(TeacherDefaultPeriodVM vm, DateTime realTutorPeriod)
        {
            var entity = realTutorPeriod.ToEntity(vm);
            _db.TeachersRealTutorPeriods.Add(entity);
            _db.SaveChanges();
        }

        private void DeleteRealTutorPeriod(TeacherDefaultPeriodVM vm, List<DateTime> dbRealTutor, List<DateTime> newList)
        {
            var needDeletedTime = dbRealTutor.Except(newList).ToList();
            foreach (var delete in needDeletedTime)
            {
                var delEntity = _db.TeachersRealTutorPeriods
                                    .Where(x => x.TeacherId == vm.TeacherId && x.TutorStartTime == delete)
                                    .FirstOrDefault();
                if (delEntity != null)
                {
                    _db.TeachersRealTutorPeriods.Remove(delEntity);
                    _db.SaveChanges();
                }
            }
        }

        private void CreateDefaultTutorPeriod(TeacherDefaultPeriodVM vm, List<int> dbList, KeyValuePair<int, List<int>> kvp)
        {
            foreach (int period in kvp.Value)
            {
                if (!dbList.Contains(period))
                {
                    var entity = vm.ToEntity(kvp.Key, period);
                    _db.TeachersDefaultTutorPeriods.Add(entity);
                    _db.SaveChanges();
                }
            }
        }

        private void DeleteDefaultTutorPeriod(TeacherDefaultPeriodVM vm, List<int> dbList, KeyValuePair<int, List<int>> kvp)
        {
            var needDeletePeriod = dbList.Except(kvp.Value).ToList();
            foreach (var item in needDeletePeriod)
            {
                var deleteList = _db.TeachersDefaultTutorPeriods
                .Where(x => x.TeacherId == vm.TeacherId && x.WeekdayId == kvp.Key && x.PeriodId == item);
                _db.TeachersDefaultTutorPeriods.RemoveRange(deleteList);
                _db.SaveChanges();
            }
        }

        public async Task<TeacherDefaultPeriodDto> GetDefaultTutorPeriod(int teacherId)
        {
            try
            {
                var awaitList = _db.TeachersDefaultTutorPeriods.Where(x => x.TeacherId == teacherId).ToListAsync();
                var list = await awaitList;

                var dto = new TeacherDefaultPeriodDto();
                foreach (var item in list)
                {
                    switch (item.WeekdayId)
                    {
                        case 1:
                            dto.Mon.Add((int)item.PeriodId);
                            break;
                        case 2:
                            dto.Tue.Add((int)item.PeriodId);
                            break;
                        case 3:
                            dto.Wed.Add((int)item.PeriodId);
                            break;
                        case 4:
                            dto.Thu.Add((int)item.PeriodId);
                            break;
                        case 5:
                            dto.Fri.Add((int)item.PeriodId);
                            break;
                        case 6:
                            dto.Sat.Add((int)item.PeriodId);
                            break;
                        case 7:
                            dto.Sun.Add((int)item.PeriodId);
                            break;
                        default:
                            throw new Exception("找不到此星期");
                    }
                }
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string MemberSelectCourse(MemberSelectCourseVM vm)
        {
            //找到符合會員選取時段的資料
            var realPeriodData = _db.TeachersRealTutorPeriods.Where(x => x.TeacherId == vm.TeacherId && x.TutorStartTime == vm.TutorTime).FirstOrDefault();

            if (realPeriodData == null) throw new Exception("沒有這個資料");

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    //將資料處理後加入資料庫中
                    int tutorPeriodId = realPeriodData.Id;
                    var entity = vm.ToEntity(tutorPeriodId);
                    _db.MembersTutorRecords.Add(entity);
                    _db.SaveChanges();

                    //更改符合時段資料的狀態將他變為已預約
                    realPeriodData.Status = true;
                    _db.SaveChanges();

                    //TODO 更改會員購買老師課堂的數量
                    EditTutorStock(vm);

                    transaction.Commit();
                    return "Success";
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        private void EditTutorStock(MemberSelectCourseVM vm)
        {
            var entity = _db.MembersTutorStock.Where(x => x.MemberId == vm.MemberId && x.TeacherId == vm.TeacherId).FirstOrDefault();
            if (entity == null) throw new Exception("您擁有此老師的課程數量已經用完囉！");

            //如果進來的庫存僅剩2 就update庫存-1
            if (entity.TutorStock > 1)
            {
                entity.TutorStock--;
            }
            //否則就刪除此筆資料
            else
            {
                _db.MembersTutorStock.Remove(entity);
            }
            _db.SaveChanges();
        }

        public Task<List<MemberTutorStockDto>> GetTutorStock(int memberId)
        {
            var query = from mts in _db.MembersTutorStock
                        join t in _db.TeacherId on mts.TeacherId equals t.Id
                        join m in _db.Members on t.MemberId equals m.Id
                        where mts.MemberId == memberId
                        select new MemberTutorStockDto()
                        {
                            TeacherId = mts.TeacherId,
                            TeacherName = m.Name,
                            TutorStock = mts.TutorStock
                        };

            return query.ToListAsync();
        }
    }
}
