namespace SimpleProject.Domain.Dtos.QRTransferDto;

public class QrTransferInitRequest
{
    public string Code { get; set; } = null!;
    public int? FromOwnerUserId { get; set; }
    public int? FromDealerId { get; set; }
    public int? ToOwnerUserId { get; set; }
    public int ExpiresInMinutes { get; set; } = 60;
}