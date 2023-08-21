using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nsl_Api.Models.Datas;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models.Infra.Repositories.EFRepositories;
using Nsl_Api.Models.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nsl_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BackMembersController : ControllerBase
	{
		private readonly NSL_DBContext _db;
		public BackMembersController(NSL_DBContext db)
		{
			_db = db;
		}

		// GET: api/<MembersController>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<MemberListItemDto>>> GetList(string? search, [FromQuery(Name ="role")] int[]? role)
		{
			IMemberRepo repo = new BackMembersRepository(_db);
			var list = new MemberListData();
			try
			{
				list.Data =await repo.GetList(search, role);
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
			return list.Data;
		}

		// DELETE api/<MembersController>/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			if (_db.Articles == null)
			{
				return NotFound();
			}
			IMemberRepo repo = new BackMembersRepository(_db);
			string result = await repo.Delete(id);


			return Content(result);
		}
	}
}
