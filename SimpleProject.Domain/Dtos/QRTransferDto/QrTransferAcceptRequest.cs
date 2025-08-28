namespace SimpleProject.Domain.Dtos.QRTransferDto;

public class QrTransferAcceptRequest
{
    public string Token { get; set; } = null!;
    public int ToOwnerUserId { get; set; }
}