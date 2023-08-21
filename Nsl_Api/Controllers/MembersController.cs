using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nsl_Api.Models.EFModels;
using Nsl_Api.Models;
using Nsl_Api.Models.Datas;
using Nsl_Api.Models.DTOs;
using Nsl_Api.Models.Infra.Repositories.EFRepositories;
using Nsl_Api.Models.Interfaces;
using Nsl_Api.Models.Dtos;
using Nsl_Api.Models.Interfaces;
using Nsl_Api.Models.Infra.Repositories.ADORepositories;

namespace Nsl_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly NSL_DBContext _db;

        public MembersController(NSL_DBContext db)
        {
            _db = db;
        }

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Members>>> GetMembers()
        {
          if (_db.Members == null)
          {
              return NotFound();
          }
            return await _db.Members.ToListAsync();
        }

        [HttpGet]
        [Route("GetMemberFile")]
        public async Task<ActionResult<IEnumerable<MemberFileDto>>> GetMemberFile(int memberId)
        {
            IMemberFileRepo repo = new MemberFileRespository(_db);
            if (_db.Members == null)
            {
                return NotFound();
            }
            var list = new MemberFile();
            try
            {
                list.Data = await repo.GetMemberFile(memberId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return list.Data;
        }


        [HttpGet]
        [Route("GetMemberDetail")]
        public async Task<ActionResult<IEnumerable<MemberDetailDto>>> GetMemberDetail(int? consumeId)
        {
            IMemberFileRepo repo = new MemberFileRespository(_db);
            if (_db.Members == null)
            {
                return NotFound();
            }
            var list = new MemberDetailData();
            try
            {
                list.Data = await repo.GetMemberDetail(consumeId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return list.Data;
        }



        

        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Members>> GetMembers(int id)
        {
          if (_db.Members == null)
          {
              return NotFound();
          }
            var members = await _db.Members.FindAsync(id);

            if (members == null)
            {
                return NotFound();
            }

            return members;
        }

        // PUT: api/Members/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMembers(int id, Members members)
        {
            if (id != members.Id)
            {
                return BadRequest();
            }

            _db.Entry(members).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        
        [HttpPost]
        public async Task<ActionResult<Members>> PostMembers(Members members)
        {
          if (_db.Members == null)
          {
              return Problem("Entity set 'NSL_DBContext.Members'  is null.");
          }
            _db.Members.Add(members);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetMembers", new { id = members.Id }, members);
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembers(int id)
        {
            if (_db.Members == null)
            {
                return NotFound();
            }
            var members = await _db.Members.FindAsync(id);
            if (members == null)
            {
                return NotFound();
            }

            _db.Members.Remove(members);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool MembersExists(int id)
        {
            return (_db.Members?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
