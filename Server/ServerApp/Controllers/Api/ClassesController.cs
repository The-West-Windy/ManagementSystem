using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClassesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClassesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/classes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingClass>>> GetClasses()
        {
            var classes = await _context.Classes
                .Include(c => c.Coach)
                .ToListAsync();

            return Ok(classes);
        }
    }
}
