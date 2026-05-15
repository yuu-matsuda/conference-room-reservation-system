namespace RoomApi.Models;

public class Room
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public required int Capacity { get; set; }
    public string? Description { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}