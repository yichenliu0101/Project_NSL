using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nsl_Api.Models.Datas;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Infra.Repositories.DapperRepositories;
using Nsl_Api.Models.Infra.Repositories.EFRepositories;
using Nsl_Api.Models.Interfaces;
using Nsl_Api.Models.ViewModels;
using Nsl_Api.Models.Interfaces;
using NuGet.Protocol.Plugins;


namespace Nsl_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherTutorController : ControllerBase
    {
        private readonly NSL_DBContext _context;
        private readonly IConfiguration _configuration;
		private readonly ITeacherRepo _repo;

		public TeacherTutorController(NSL_DBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _repo = new TeacherRepositoryEF(context);

		}

        [HttpGet]
        [Route("TeacherTutorRecord")]
        public async Task<ActionResult<IEnumerable<TeacherTutorRecordDto>>> GetTutorRecord(int teacherId)
        {
            var repo = new TeacherRepositoriesDapper(_configuration);
            var list = new TeacherTutorRecordData();
            try
            {
                list.Data = await repo.GetTutorRecord(teacherId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return list.Data;
        }

		[HttpPut]
		[Route("UpdateTutorRecordStatus")]
		public ResultDto UpdateTutorRecordStatus(int tutorRecordId)
		{
			var result = new ResultDto() { isSuccess = true };
			try
			{
                var tutorRecord = _context.MembersTutorRecords.Find(tutorRecordId);

                tutorRecord.Status = true;
                _context.SaveChanges();
            }
			catch(Exception ex) 
			{
				result.isSuccess = false;
				result.errMsg = ex.Message;
			}
			return result;
		}

        [HttpGet]
		[Route("TeacherComments")]
		public async Task<ActionResult<IEnumerable<TeacherCommentsDto>>> GetComments(int id)
		{
			var repo = new TeacherRepositoriesDapper(_configuration);
			var list = new TeacherCommentsData();
			try
			{
				list.Data = await repo.GetComments(id);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
			return list.Data;
		}

		[HttpGet]
        [Route("TeacherGetPrice")]
        public async Task<ActionResult<IEnumerable<TeacherGetPriceDto>>> GetPrice(int teacherId)
        {

            var repo = new TeacherRepositoriesDapper(_configuration);
            var list = new TeacherGetPriceData();
            try
            {
                list.Data = await repo.GetPrice(teacherId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return list.Data;

        }

		[HttpPost]
		[Route("EditTeacherPrice")]
		public Task<ResultDto> EditTeacherPrice(TeacherPriceVM vm)
		{
			var result = new ResultDto() { isSuccess = true };
			try
			{
				_repo.EditTeacherPrice(vm);

			}
			catch (Exception ex)
			{
				result.isSuccess = false;
				result.errMsg = ex.Message;
			}
			return Task.FromResult(result);
		}

		[HttpGet]
		[Route("GetTeacherTags")]
		public async Task<ActionResult<IEnumerable<TeacherTagsDto>>> GetTeacherTags(int teacherId)
		{
			var list = new TeacherTagsData();
			try
			{
				list.Data = await _repo.GetTeacherTags(teacherId);

				return list.Data;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		[Route("AutoCompleteTags")]
		public async Task<ActionResult<IEnumerable<Tags>>> GetAutoCompleteTags(string input)
		{
			var list = new AutoCompleteTagsData();
			try
			{
				list.Data = await _repo.GetAutoCompleteTags(input);

				return list.Data;
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		[Route("CreateTeacherTags")]
		public Task<ResultDto> CreateTeacherTags(TeacherCreate dto)
		{
			var result = new ResultDto() { isSuccess = true };
			if (dto == null)
			{
				result.isSuccess = false;
				result.errMsg = "資料未傳送進來";
				return Task.FromResult(result);
			}

			try
			{				
				_repo.CreateTeacherTags(dto);
			}

			catch (Exception ex)
			{
				result.isSuccess = false;
				result.errMsg = ex.Message;
			}
			return Task.FromResult(result);
		}

		[HttpPost]
		[Route("EditTeacherTags")]
		public Task<ResultDto> EditTeacherTags(TeacherTagsDto dto)
		{
			var result = new ResultDto() { isSuccess = true };
			try
			{
				_repo.EditTeacherTags(dto);

			}
			catch (Exception ex)
			{
				result.isSuccess = false;
				result.errMsg = ex.Message;
			}
			return Task.FromResult(result);
		}

        [HttpDelete]
        [Route("DeleteTeachersTags")]
        public Task<ResultDto> DeleteTeachersTags(TeachersTagsVM vm)
        {
            var result = new ResultDto() { isSuccess = true };
            try
            {
                _repo.DeleteTeachersTags(vm);
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.errMsg = ex.Message;
            }
            return Task.FromResult(result);
        }
    }
}
