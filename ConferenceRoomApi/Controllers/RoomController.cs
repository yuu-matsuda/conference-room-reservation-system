using Microsoft.AspNetCore.Mvc;
using RoomApi.Models;
using RoomApi.Dto;
using Microsoft.EntityFrameworkCore;
using RoomApi.Service;

namespace RoomApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly RoomContext _context;
    private readonly RoomService _roomService;

    public RoomController(RoomContext context, RoomService roomService)
    {
        _context = context;
        _roomService = roomService;
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
    public async Task<ActionResult<Room>> PostRoom(CreateRoomDto dto)
    {
        var result = await _roomService.CreateAsync(dto);
        if (result == null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(GetRoom), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Room>> PutRoom(long id, CreateRoomDto dto)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
        {
            return NotFound();
        }
        
            room.Name = dto.Name;
            room.Capacity = dto.Capacity;
            room.Description = dto.Description;
            room.UpdatedAt = DateTime.UtcNow;
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
