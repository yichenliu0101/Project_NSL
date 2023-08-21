using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nsl_Api.Models;
using Nsl_Api.Models.EFModels;

namespace Nsl_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly NSL_DBContext _db;

        public CouponsController(NSL_DBContext db)
        {
            _db = db;
        }

        // GET: api/Coupons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coupons>>> GetCoupons()
        {
          if (_db.Coupons == null)
          {
              return NotFound();
          }
            return await _db.Coupons.ToListAsync();
        }

        // GET: api/Coupons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Coupons>> GetCoupons(int id)
        {
          if (_db.Coupons == null)
          {
              return NotFound();
          }
            var coupons = await _db.Coupons.FindAsync(id);

            if (coupons == null)
            {
                return NotFound();
            }

            return coupons;
        }


        [Route("UnusedCoupons")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coupons>>> GetUnusedCoupon(int memberid)
        {
            var userdcouponlist = _db.CouponUsageHistory.Where(o => o.MemberId == memberid).Select(o => o.CouponId).ToList();
            var unusedcoupon = await _db.Coupons
            .Where(o => !userdcouponlist.Contains(o.Id))
            .Select(o=>new Coupons() { 
                 Id = o.Id,
                 Name = o.Name,
                 DiscountMoney = o.DiscountMoney,
                 Description = o.Description,
                 ExpireTime = o.ExpireTime,
            }).ToArrayAsync();

            return unusedcoupon;
        }
        // PUT: api/Coupons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCoupons(int id, Coupons coupons)
        {
            if (id != coupons.Id)
            {
                return BadRequest();
            }

            _db.Entry(coupons).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CouponsExists(id))
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

        // POST: api/Coupons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Coupons>> PostCoupons(Coupons coupons)
        {
          if (_db.Coupons == null)
          {
              return Problem("Entity set 'NSL_DBContext.Coupons'  is null.");
          }
            _db.Coupons.Add(coupons);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetCoupons", new { id = coupons.Id }, coupons);
        }

        // DELETE: api/Coupons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoupons(int id)
        {
            if (_db.Coupons == null)
            {
                return NotFound();
            }
            var coupons = await _db.Coupons.FindAsync(id);
            if (coupons == null)
            {
                return NotFound();
            }

            var usage = _db.CouponUsageHistory.Where(x => x.CouponId == id).ToList();
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _db.RemoveRange(usage);
                    await _db.SaveChangesAsync();
                    _db.Coupons.Remove(coupons);
                    await _db.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return NoContent();
                }
            }

            return NoContent();
        }

        private bool CouponsExists(int id)
        {
            return (_db.Coupons?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
