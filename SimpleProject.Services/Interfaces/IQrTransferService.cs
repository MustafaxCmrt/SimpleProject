using SimpleProject.Domain.Dtos.QROwnershipDto;
using SimpleProject.Domain.Dtos.QRTransferDto;

namespace SimpleProject.Services.Interfaces;

public interface IQrTransferService
{
    Task<QrTransferInitResponse> InitAsync(QrTransferInitRequest req, CancellationToken ct);
    Task<QrOwnershipResponse> AcceptAsync(QrTransferAcceptRequest req, CancellationToken ct);
}