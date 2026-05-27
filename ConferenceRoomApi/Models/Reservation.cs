namespace RoomApi.Models;

public class Reservation
{
    public long Id { get; set; }
    public required long RoomId { get; set; }
    public required string RoomName { get; set; }
    public required string Title { get; set; }
    public required DateTime StartAt { get; set; }
    public required DateTime EndAt { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}