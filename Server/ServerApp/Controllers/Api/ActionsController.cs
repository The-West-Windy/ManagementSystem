using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerApp.Models;
using ServerApp.Models.DTOs;
using System;
using System.Threading.Tasks;

namespace ServerApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ActionsController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/actions
        [HttpPost]
        public async Task<IActionResult> CreateAction([FromBody] BorrowRequestDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Request body is null");
            }

            // Перевірка існування Librarian і Book
            var librarian = await _context.Librarians.FindAsync(dto.LibrarianID);
            var book = await _context.Books.FindAsync(dto.BookID);

            if (librarian == null || book == null)
            {
                return NotFound("Librarian or Book not found");
            }

            var borrowRequest = new BorrowRequest
            {
                LibrarianID = dto.LibrarianID,
                BookID = dto.BookID,
                Status = dto.Status,
                RequestDate = DateTime.UtcNow
            };

            _context.BorrowRequests.Add(borrowRequest);
            await _context.SaveChangesAsync();

            // Повертаємо тільки ID нового запиту
            return Ok(new { borrowRequest.ID });
        }
    }
}
