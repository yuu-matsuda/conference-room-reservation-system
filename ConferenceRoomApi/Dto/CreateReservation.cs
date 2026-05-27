namespace RoomApi.Dto;

public class CreateReservationDto
{
    public required long RoomId { get; set; }
    public required string Title { get; set; }
    public required DateTime StartAt { get; set; }
    public required DateTime EndAt { get; set; }
}