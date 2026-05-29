using System;
using Microsoft.AspNetCore.Mvc;
using RoomApi.Models;
using RoomApi.Dto;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
            return NotFound("会議室が見つかりません");
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

        if(reservation.StartAt > reservation.EndAt){
            return BadRequest("時刻の設定に誤りがあります");
        }
        var reservationList = await _context.Reservations.ToListAsync();
        var result = reservationList.Where(r => r.RoomId == reservation.RoomId).ToList();
        var errorReservation = result
        .Where(r => reservation.StartAt < r.EndAt && r.StartAt < reservation.EndAt)
        .ToList();

        if(errorReservation.Count > 0){
            return BadRequest("設定された時刻には既に予約があります");
        }

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
