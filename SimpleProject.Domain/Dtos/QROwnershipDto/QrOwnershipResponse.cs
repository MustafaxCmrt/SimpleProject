namespace SimpleProject.Domain.Dtos.QROwnershipDto;

public class QrOwnershipResponse
{
    public int OwnershipId { get; set; }
    public int CollarId { get; set; }
    public int OwnerUserId { get; set; }
    public DateTime ActivatedAt { get; set; }
    public bool IsActive { get; set; }
}