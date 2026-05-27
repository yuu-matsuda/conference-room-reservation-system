using Microsoft.AspNetCore.Mvc;
using RoomApi.Models;
using RoomApi.Dto;
using Microsoft.EntityFrameworkCore;

namespace RoomApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly RoomContext _context;

    public ReservationController(RoomContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
    {
        return await _context.Reservations.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Reservation>> GetReservation(long id)
    {
        var room = await _context.Reservations.FindAsync(id);
        if (room == null)
        {
            return NotFound();
        }
        return room;
    }

    [HttpPost]
    public async Task<ActionResult<Reservation>> PostRoom(CreateReservationDto CreateReservation)
    {
        var room = await _context.Rooms.FindAsync(CreateReservation.RoomId);
        if (room == null)
        {
            return NotFound("見つかりません");
        }

        var reservation = new Reservation
        {
            RoomName = room.Name,
            RoomId = CreateReservation.RoomId,
            Title = CreateReservation.Title,
            StartAt = CreateReservation.StartAt,
            EndAt = CreateReservation.EndAt,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Reservation>> PutRoom(long id, CreateReservationDto reservationDto)
    {
        var room = await _context.Rooms.FindAsync(reservationDto.RoomId);
        if (room == null)
        {
            return NotFound();
        }

        var reservation = new Reservation
        {
            Id = id,
            RoomId = reservationDto.RoomId,
            RoomName = room.Name,
            Title = reservationDto.Title,
            StartAt = reservationDto.StartAt,
            EndAt = reservationDto.EndAt,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
         _context.Entry(reservation).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(long id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return NotFound();
        }
        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
