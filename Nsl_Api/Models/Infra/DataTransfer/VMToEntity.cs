using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.ViewModels;

namespace Nsl_Api.Models.Infra.DataTransfer
{
    public static class VMToEntity
    {
        public static TeacherId MemberIdToTeacherId(this int memberId)
        {
            return new TeacherId { MemberId = memberId };
        }

        public static TeachersApply ToEntity(this TeacherApplyVM vm) 
        {
            return new TeachersApply
            {
                CategoryId = vm.Category,
                TutorExperienceId = vm.TutorExperience,
                WorkStatusId = vm.WorkStatus,
                RevenueTargetId = vm.RevenueTarget,
                TutorHoursOfWeekId = vm.TutorHoursOfWeek,
                Intro = vm.Intro,
                ApplyTime = DateTime.Now,
            };
        }

        public static TeachersRealTutorPeriods ToEntity(this TeacherCreateCourseVM vm)
        {
            return new TeachersRealTutorPeriods
            {
                TeacherId = vm.TeacherId,
                TutorStartTime = vm.StartDate.AddHours(vm.StartTime-1),
                Status = false,
            };
        }

        public static TeachersDefaultTutorPeriods ToEntity(this TeacherDefaultPeriodVM vm, int week, int period)
        {
            return new TeachersDefaultTutorPeriods
            {
                TeacherId = vm.TeacherId,
                WeekdayId = week,
                PeriodId = period
            };
        }

		public static TeachersRealTutorPeriods ToEntity(this DateTime startTime, TeacherDefaultPeriodVM vm)
        {
            return new TeachersRealTutorPeriods
            {
                TeacherId = vm.TeacherId,
                TutorStartTime = startTime,
                Status = false,
            };
        }

        public static MembersTutorRecords ToEntity(this MemberSelectCourseVM vm, int id)
        {
            return new MembersTutorRecords
            {
                MemberId = vm.MemberId,
                TeacherTutorPeriodId = id,
                Status = false,
            };
        }
	}
}
