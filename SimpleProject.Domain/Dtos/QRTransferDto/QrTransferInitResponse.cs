namespace SimpleProject.Domain.Dtos.QRTransferDto;

public class QrTransferInitResponse
{
    public int TicketId { get; set; }
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public string Status { get; set; } = "PENDING";
}