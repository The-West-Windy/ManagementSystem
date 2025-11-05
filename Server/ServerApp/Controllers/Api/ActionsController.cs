using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;
using ServerApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Controllers.Api
{
    [Authorize(Roles = "Admin")] // доступ тільки для адміністратора
    [Route("api/[controller]")]
    [ApiController]
    public class ActionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ActionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/actions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowRequest>>> GetActions()
        {
            var actions = await _context.BorrowRequests
                                        .Include(br => br.Librarian)
                                        .Include(br => br.Book)
                                        .ToListAsync();
            return Ok(actions);
        }

        // GET: /api/actions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowRequest>> GetAction(int id)
        {
            var action = await _context.BorrowRequests
                                       .Include(br => br.Librarian)
                                       .Include(br => br.Book)
                                       .FirstOrDefaultAsync(br => br.ID == id);
            if (action == null)
                return NotFound();

            return Ok(action);
        }

        // POST: /api/actions
        [HttpPost]
        public async Task<IActionResult> CreateAction([FromBody] BorrowRequestDto dto)
        {
            if (dto == null)
                return BadRequest("Request body is null");

            var librarian = await _context.Librarians.FindAsync(dto.LibrarianID);
            var book = await _context.Books.FindAsync(dto.BookID);

            if (librarian == null || book == null)
                return NotFound("Librarian or Book not found");

            var borrowRequest = new BorrowRequest
            {
                LibrarianID = dto.LibrarianID,
                BookID = dto.BookID,
                Status = dto.Status,
                RequestDate = DateTime.UtcNow
            };

            _context.BorrowRequests.Add(borrowRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAction), new { id = borrowRequest.ID }, borrowRequest);
        }

        // PUT: /api/actions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAction(int id, [FromBody] BorrowRequestDto dto)
        {
            if (id != dto.ID)
                return BadRequest("ID mismatch");

            var borrowRequest = await _context.BorrowRequests.FindAsync(id);
            if (borrowRequest == null)
                return NotFound();

            // Оновлюємо дані
            borrowRequest.LibrarianID = dto.LibrarianID;
            borrowRequest.BookID = dto.BookID;
            borrowRequest.Status = dto.Status;

            _context.Entry(borrowRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.BorrowRequests.Any(e => e.ID == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: /api/actions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAction(int id)
        {
            var borrowRequest = await _context.BorrowRequests.FindAsync(id);
            if (borrowRequest == null)
                return NotFound();

            _context.BorrowRequests.Remove(borrowRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
