using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // захист ендпоінту
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
    }
}
