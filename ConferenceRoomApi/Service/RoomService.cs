using RoomApi.Models;
using RoomApi.Dto;
using Microsoft.EntityFrameworkCore;

namespace RoomApi.Service;

public class RoomService
{
    private readonly RoomContext _context;
    public RoomService(RoomContext context)
    {
        _context = context;
    }

    public async Task<Room?> CreateAsync(CreateRoomDto dto)
    {
        var room = new Room
        {
            Name = dto.Name,
            Capacity = dto.Capacity,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();
        return room;
    }
}