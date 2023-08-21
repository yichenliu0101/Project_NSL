using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nsl_Api.Models.Datas;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Infra.Repositories.DapperRepositories;
using Nsl_Api.Models.Infra.Repositories.EFRepositories;
using Nsl_Api.Models.Interfaces;
using Nsl_Api.Models.ViewModels;
using Nsl_Api.Models.Dtos;
using Nsl_Api.Models.Infra.Repositories.ADORepositories;
using Nsl_Api.Models.Interfaces;
namespace Nsl_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TeachersAppliesController : ControllerBase
	{
		private readonly NSL_DBContext _context;
		private readonly IConfiguration _configuration;
        private readonly ITeacherRepo _repo;

        public TeachersAppliesController(IConfiguration configuration, NSL_DBContext context)
		{
			_context = context;
			_configuration = configuration;
            _repo = new TeacherRepositoryEF(context);
        }
		[HttpGet]
		[Route("GetAllApplyList")]
		public async Task<ActionResult<IEnumerable<TeacherApplyListDto>>> GetAllApplyList(string? search, int? searchLan)
		{
			//ITeacherRepo repo = new BackTeacherRepository(_context);
			var repo = new Models.Infra.Repositories.EFRepositories.TeacherRepositoryEF(_context);
			if (_context.TeachersApply == null)
			{
				return NotFound();
			}
			var list = new TeacherApplyListData();
			try
			{
				list.Data = await repo.GetAllApplyList(search, searchLan);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
			return list.Data;
		}

        [HttpDelete]
        [Route("DeleteApply")]
        public Task<ResultDto> DeleteApply(int teacherId)
        {
            var result = new ResultDto() { isSuccess = true };
            try
            {
                _repo.DeleteApply(teacherId);
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.errMsg = ex.Message;
            }
            return Task.FromResult(result);
        }

		[HttpGet]
		[Route("VerifyIsApplied")]
		public Task<ResultDto> VerifyIsApplied(int memberId)
		{
            var result = new ResultDto() { isSuccess = true };
            //確認會員是否已經申請過
            var repo = new FrontTeacherRepositoriesEF(_context);
			try
			{
                repo.GetTeacherId(memberId);
            }
			catch(Exception ex)
			{
				result.isSuccess = false;
				result.errMsg = ex.Message;
			}
			return Task.FromResult(result);
        }

        [HttpPost]
		[Route("GetApplyFormData")]
		public Task<ResultDto> GetApplyFormData(TeacherApplyVM formData)
		{
			var result = new ResultDto() { isSuccess = true };
			if (formData == null)
			{
				result.isSuccess = false;
				result.errMsg = "資料未傳送進來";
				return Task.FromResult(result);
			}

			var repo = new FrontTeacherRepositoriesEF(_context);
			try
			{
				//創立老師申請資料至資料庫
				repo.CreateTeacherApplyTransaction(formData);
				
			}
			catch (Exception ex)
			{
				result.isSuccess = false;
				result.errMsg = ex.Message;
			}
			return Task.FromResult(result);
		}
		[HttpGet]
		[Route("GetInfoData")]
		public async Task<ActionResult<TeacherInfoDto>> GetInfoData(int teacherId)
		{
			var repo = new TeacherInfoRepo(_configuration);
			var dto = repo.GetResume(teacherId);

			if (dto == null)
			{
				return NoContent();
			}
			return await dto;
		}

		[HttpGet]
		[Route("GetTag")]
		public async Task<ActionResult<List<TagDto>>> GetTag(int teacherId)
		{
			var repo = new TeacherInfoRepo(_configuration);
			var dto = repo.GetTag(teacherId);

			if (dto == null)
			{
				return null;
			}
			return await dto;
		}
	}
}
