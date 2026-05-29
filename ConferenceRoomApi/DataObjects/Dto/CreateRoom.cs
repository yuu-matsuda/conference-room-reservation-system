namespace RoomApi.Dto;

public class CreateRoomDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required int Capacity { get; set; }
    public string? Description { get; set; }
}