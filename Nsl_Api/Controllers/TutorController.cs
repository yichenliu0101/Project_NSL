using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nsl_Api.Models.Datas;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Infra.DataTransfer;
using Nsl_Api.Models.Infra.Repositories.EFRepositories;
using Nsl_Api.Models.Interfaces;
using Nsl_Api.Models.Service;
using Nsl_Api.Models.ViewModels;

namespace Nsl_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TutorController : ControllerBase
	{
		private readonly NSL_DBContext _db;
		private readonly ITutorRepo _repo;
		private readonly TutorService _service;
		public TutorController(NSL_DBContext db)
		{
			_db = db;
			_repo = new TutorRepositoriesEF(db);
			_service = new TutorService(db, _repo);
		}

		[HttpGet]
		[Route("TeacherTutorPeriod")]
		public async Task<ActionResult<IEnumerable<TutorPeriodFullCalendarDto>>> GetTutorPeriodData(int teacherId)
		{
			var list = new TutorPeriodFullCalendarData();
			try
			{
				list.Data = await _repo.GetTutorPeriodData(teacherId);
				return list.Data;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		[Route("MemberTutorRecord")]
		public async Task<ActionResult<IEnumerable<TutorPeriodFullCalendarDto>>> GetMemberTutorPeriodRecord(int memberId)
		{
			var list = new TutorPeriodFullCalendarData();
			try
			{
				list.Data = await _repo.GetMemberTutorPeriodRecord(memberId);
				return list.Data;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		[Route("GetTeacherTutorPeriodData")]
		public async Task<ActionResult<IEnumerable<TutorPeriodFullCalendarDto>>> GetTeacherTutorPeriodData(int teacherId)
		{
			var list = new TutorPeriodFullCalendarData();
			try
			{
				list.Data = await _repo.GetTeacherTutorPeriodData(teacherId);

				return list.Data;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		[Route("CreateNewCourse")]
		public Task<ResultDto> CreateNewCourse(TeacherCreateCourseVM vm)
		{
			var result = new ResultDto() { isSuccess = true };
			var service = new TutorService(_db, _repo);
			try
			{
				var entity = vm.ToEntity();
				service.CreateNewCourse(entity);
			}
			catch (Exception ex)
			{
				result.isSuccess = false;
				result.errMsg = ex.Message;
			}
			return Task.FromResult(result);
		}

		[HttpDelete]
		[Route("DeleteCourse")]
		public Task<ResultDto> DeleteCourse(TeacherDeleteCourseVM vm)
		{
			var result = new ResultDto() { isSuccess = true };
			try
			{
				vm.StartTime = vm.StartTime.AddHours(8);
				_repo.DeleteCourse(vm);
			}
			catch (Exception ex)
			{
				result.isSuccess = false;
				result.errMsg = ex.Message;
			}
			return Task.FromResult(result);
		}

		[HttpPost]
		[Route("EditDefaultTutorPeriod")]
		public Task<ResultDto> EditDefaultTutorPeriod(TeacherDefaultPeriodVM vm)
		{
			var result = new ResultDto() { isSuccess = true };
			try
			{
				_repo.EditDefaultTutorPeriod(vm);

			}
			catch (Exception ex)
			{
				result.isSuccess = false;
				result.errMsg = ex.Message;
			}
			return Task.FromResult(result);
		}

		[HttpGet]
		[Route("GetDefaultTutorPeriod")]
		public async Task<ActionResult<TeacherDefaultPeriodDto>> GetDefaultTutorPeriod(int teacherId)
		{
			try
			{
				var dto = await _repo.GetDefaultTutorPeriod(teacherId);
				return dto;
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		[Route("MemberSelectCourse")]
		public Task<ResultDto> MemberSelectCourse(MemberSelectCourseVM vm)
		{
			var result = new ResultDto() { isSuccess = true };
			try
			{ 
				vm.TutorTime = vm.TutorTime.AddHours(8);
				_service.MemberSelectCourse(vm);
			}
			catch(Exception ex)
			{
				result.isSuccess = false;
				result.errMsg = ex.Message;
			}
			return Task.FromResult(result);
		}

		[HttpGet]
		[Route("GetTutorStock")]
		public async Task<ActionResult<IEnumerable<MemberTutorStockDto>>> GetTutorStock(int memberId)
		{
			var list = new MemberTutorStockListData();
			try
			{
				list.Data = await _repo.GetTutorStock(memberId);
				return list.Data;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		} 
	}
}
