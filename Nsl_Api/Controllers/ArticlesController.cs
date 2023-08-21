using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nsl_Api.Models.EFModels;

namespace Nsl_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly NSL_DBContext _db;

        public ArticlesController(NSL_DBContext db)
        {
            _db = db;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Articles>>> GetArticles()
        {
          if (_db.Articles == null)
          {
              return NotFound();
          }
            return await _db.Articles.OrderByDescending(x=>x.ModifiedTime).ToListAsync();
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Articles>> GetArticles(int id)
        {
          if (_db.Articles == null)
          {
              return NotFound();
          }
            var articles = await _db.Articles.FindAsync(id);

            if (articles == null)
            {
                return NotFound();
            }

            return articles;
        }

        // PUT: api/Articles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticles(int id, Articles articles)
        {
            if (id != articles.Id)
            {
                return BadRequest();
            }

            _db.Entry(articles).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticlesExists(id))
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

        // POST: api/Articles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Articles>> PostArticles(Articles articles)
        {
          if (_db.Articles == null)
          {
              return Problem("Entity set 'NSL_DBContext.Articles'  is null.");
          }
            _db.Articles.Add(articles);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetArticles", new { id = articles.Id }, articles);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticles(int id)
        {
            if (_db.Articles == null)
            {
                return NotFound();
            }
            var articles = await _db.Articles.FindAsync(id);
            if (articles == null)
            {
                return NotFound();
            }

            _db.Articles.Remove(articles);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticlesExists(int id)
        {
            return (_db.Articles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
