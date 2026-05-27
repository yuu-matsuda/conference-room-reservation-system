using Microsoft.AspNetCore.Mvc;
using RoomApi.Models;
using RoomApi.Dto;
using Microsoft.EntityFrameworkCore;

namespace RoomApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly RoomContext _context;

    public RoomController(RoomContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
    {
        return await _context.Rooms.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Room>> GetRoom(long id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
        {
            return NotFound();
        }
        return room;
    }

    [HttpPost]
    public async Task<ActionResult<Room>> PostRoom(CreateRoomDto createRoom)
    {
        var room = new Room
        {
            Name = createRoom.Name,
            Capacity = createRoom.Capacity,
            Description = createRoom.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Room>> PutRoom(long id, CreateRoomDto createRoom)
    {
        var room = new Room
        {
            Id = id,
            Name = createRoom.Name,
            Capacity = createRoom.Capacity,
            Description = createRoom.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
         _context.Entry(room).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(long id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
        {
            return NotFound();
        }
        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
