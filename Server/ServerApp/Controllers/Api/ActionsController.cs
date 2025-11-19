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

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

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
                BorrowerName = dto.BorrowerName.Trim(),
                Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim(),
                Status = BorrowRequestStatus.Pending,
                RequestDate = DateTime.UtcNow
            };

            _context.BorrowRequests.Add(borrowRequest);
            await _context.SaveChangesAsync();

            return Ok(new { borrowRequest.ID });
        }
    }
}
