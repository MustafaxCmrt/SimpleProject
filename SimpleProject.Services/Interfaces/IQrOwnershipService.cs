using SimpleProject.Domain.Dtos.QrClaimDto;
using SimpleProject.Domain.Dtos.QROwnershipDto;

namespace SimpleProject.Services.Interfaces;

public interface IQrOwnershipService
{
    Task<QrOwnershipResponse> ClaimAsync(QrClaimRequest req, CancellationToken ct);
}