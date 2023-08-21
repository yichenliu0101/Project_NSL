using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nsl_Api.Models.Datas;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Infra.Repositories.DapperRepositories;
using Nsl_Api.Models.Infra.Repositories.EFRepositories;
using Nsl_Api.Models.Interfaces;
using Nsl_Api.Models.ViewModels;

namespace Nsl_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersResumesController : ControllerBase
    {
		private readonly NSL_DBContext _context;
		private readonly IConfiguration _configuration;
        private readonly ITeacherRepo _repo;

        public TeachersResumesController(IConfiguration configuration, NSL_DBContext context)
		{
			_context = context;
			_configuration = configuration;
            _repo = new TeacherRepositoryEF(context);
        }

        [HttpGet]
        [Route("GetResumeList")]
        public async Task<ActionResult<IEnumerable<TeacherResumeDto>>> GetResumeList(string? search)
        {
            var repo = new TeacherRepositoriesDapper(_configuration);
            var list = new TeacherResumeData();
            try
            {
                list.Data = await repo.GetResumeList(search);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return list.Data;
        }
        // GET: api/TeachersResumes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeachersResume>> GetTeachersResume(int id)
        {
          if (_context.TeachersResume == null)
          {
              return NotFound();
          }
            var teachersResume = await _context.TeachersResume.FindAsync(id);

            if (teachersResume == null)
            {
                return NotFound();
            }

            return teachersResume;
        }

        [HttpPost]
        public async Task<ActionResult<TeachersResume>> PostTeachersResume(TeachersResume teachersResume)
        {
          if (_context.TeachersResume == null)
          {
              return Problem("Entity set 'NSL_DBContext.TeachersResume'  is null.");
          }
            _context.TeachersResume.Add(teachersResume);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeachersResume", new { id = teachersResume.TeacherId }, teachersResume);
        }

        // DELETE: api/TeachersResumes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeachersResume(int id)
        {
            if (_context.TeachersResume == null)
            {
                return NotFound();
            }
            var teachersResume = await _context.TeachersResume.FindAsync(id);
            if (teachersResume == null)
            {
                return NotFound();
            }

            _context.TeachersResume.Remove(teachersResume);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        [Route("BackTeacherComments")]
        public async Task<ActionResult<IEnumerable<TeacherCommentsDto>>> BackGetComments(int id)
        {
            var repo = new TeacherRepositoriesDapper(_configuration);
            var list = new TeacherCommentsData();
            try
            {
                list.Data = await repo.BackGetComments(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return list.Data;
        }

        [HttpDelete]
        [Route("DeleteComment")]
        public Task<ResultDto> DeleteComment(int id)
        {
            var result = new ResultDto() { isSuccess = true };
            try
            {
                _repo.DeleteComment(id);
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.errMsg = ex.Message;
            }
            return Task.FromResult(result);
        }

        [HttpDelete]
        [Route("DeleteResume")]
        public Task<ResultDto> DeleteResume(int teacherId)
        {
            var result = new ResultDto() { isSuccess = true };
            try
            {
                _repo.DeleteResume(teacherId);
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
