using RoomApi.Models;
using RoomApi.Dto;
using Microsoft.EntityFrameworkCore;

namespace RoomApi.Service;

public class ReservationService
{
    private readonly RoomContext _context;
    public ReservationService(RoomContext context)
    {
        _context = context;
    }

    public async Task<(Reservation? Reservation, string? Message)> CreateAsync(CreateReservationDto dto)
    {
        var room = await _context.Rooms.FindAsync(dto.RoomId);
        if (room == null)
        {
            return (null, "会議室が見つかりません");
        }

        var reservation = new Reservation
        {
            RoomName = room.Name,
            RoomId = dto.RoomId,
            Title = dto.Title,
            StartAt = DateTime.SpecifyKind(dto.StartAt, DateTimeKind.Utc),
            EndAt = DateTime.SpecifyKind(dto.EndAt, DateTimeKind.Utc),        
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        if(reservation.StartAt > reservation.EndAt){
            return (null, "時刻の設定に誤りがあります"); 
        }
        var reservationList = await _context.Reservations.ToListAsync();
        var result = reservationList.Where(r => r.RoomId == reservation.RoomId).ToList();
        var errorReservation = result
        .Where(r => reservation.StartAt < r.EndAt && r.StartAt < reservation.EndAt)
        .ToList();

        if(errorReservation.Count > 0){
           return (null, "設定された時刻には既に予約があります");
        }

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        return (reservation, null);
    }
}