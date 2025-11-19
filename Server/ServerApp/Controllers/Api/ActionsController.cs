using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerApp.Models;
using ServerApp.Models.DTOs;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            if (dto.Status != BorrowRequestStatus.Pending)
            {
                return BadRequest("New requests must be created with a Pending status.");
            }

            var claimValue = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(claimValue, out var librarianId))
            {
                return Unauthorized("Unable to determine the current librarian.");
            }

            var librarian = await _context.Librarians.FindAsync(librarianId);
            var book = await _context.Books.FindAsync(dto.BookID);

            if (librarian == null || book == null)
            {
                return NotFound("Librarian or Book not found");
            }

            var borrowRequest = new BorrowRequest
            {
                LibrarianID = librarianId,
                BookID = dto.BookID,
                BorrowerName = dto.BorrowerName.Trim(),
                Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim(),
                Status = BorrowRequestStatus.Pending,
                RequestDate = DateTime.UtcNow
            };

            _context.BorrowRequests.Add(borrowRequest);
            await _context.SaveChangesAsync();

            // Повертаємо тільки ID нового запиту
            return Ok(new { borrowRequest.ID });
        }
    }
}
