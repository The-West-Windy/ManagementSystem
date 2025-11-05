using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Api
{
    [Authorize(Roles = "Admin")] // доступ тільки для адміністратора
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetItems()
        {
            var items = await _context.Books.ToListAsync();
            return Ok(items);
        }

        // GET: /api/items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetItem(int id)
        {
            var item = await _context.Books.FindAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // POST: /api/items
        [HttpPost]
        public async Task<ActionResult<Book>> CreateItem([FromBody] Book item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Books.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItem), new { id = item.ID }, item);
        }

        // PUT: /api/items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] Book item)
        {
            if (id != item.ID)
                return BadRequest();

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: /api/items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Books.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.Books.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemExists(int id)
        {
            return _context.Books.Any(e => e.ID == id);
        }
    }
}
