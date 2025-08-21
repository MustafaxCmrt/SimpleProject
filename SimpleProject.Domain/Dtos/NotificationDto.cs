namespace SimpleProject.Domain.Dtos;

public class NotificationDto : EntityDto
{
    public int HayvanId { get; set; }
    public string Title { get; set; } = null!;
    public string? Message { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}