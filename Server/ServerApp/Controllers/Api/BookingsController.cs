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
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/bookings
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Request body is null");
            }

            // Перевірка існування Coach і TrainingClass
            var coach = await _context.Coaches.FindAsync(dto.CoachID);
            var trainingClass = await _context.Classes.FindAsync(dto.ClassID);

            if (coach == null || trainingClass == null)
            {
                return NotFound("Coach or class not found");
            }

            var booking = new Booking
            {
                CoachID = dto.CoachID,
                ClassID = dto.ClassID,
                ClientName = dto.ClientName,
                Status = dto.Status,
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Повертаємо тільки ID нового запиту
            return Ok(new { booking.ID });
        }
    }
}
