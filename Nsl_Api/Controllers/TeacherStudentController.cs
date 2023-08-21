using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nsl_Api.Models.Datas;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Infra.Repositories.DapperRepositories;
using Nsl_Api.Models.Infra.Repositories.EFRepositories;
using Nsl_Api.Models.Interfaces;
using Nsl_Api.Models.Interfaces;


namespace Nsl_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherStudentController : ControllerBase
    {
        private readonly NSL_DBContext _context;
        private readonly IConfiguration _configuration;

        public TeacherStudentController(NSL_DBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("TeacherStudentRecord")]
        public async Task<ActionResult<IEnumerable<TeacherStudentDto>>> GetStudentRecord(int teacherId)
        {
            var repo = new TeacherRepositoriesDapper(_configuration);
            var list = new TeacherStudentData();
            try
            {
                list.Data = await repo.GetStudentRecord(teacherId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return list.Data;
        }


    }
}
