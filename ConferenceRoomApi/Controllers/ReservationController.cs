using System;
using Microsoft.AspNetCore.Mvc;
using RoomApi.Models;
using RoomApi.Dto;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using RoomApi.Service;

namespace RoomApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly RoomContext _context;
    private readonly ReservationService _reservationService;

    public ReservationController(RoomContext context, ReservationService reservationService)
    {
        _context = context;
        _reservationService = reservationService;
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
    public async Task<ActionResult<Reservation>> PostReservation(CreateReservationDto dto)
    {
        var result = await _reservationService.CreateAsync(dto);

        if (result.Reservation == null)
        {
            return BadRequest(new { message = result.Message });
        }

        return CreatedAtAction(
            nameof(GetReservation),
            new { id = result.Reservation.Id },
            result.Reservation
        );
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
