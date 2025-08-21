using SimpleProject.Domain.Dtos;
namespace SimpleProject.Services.Interfaces;

public interface IQrService
{
    Task<PetPublicDto> GetPetByCodeAsync(string code, CancellationToken ct);
    Task<ScanResponse> RegisterScanAsync(string code, ScanRequest req, string ip, string? userAgent, CancellationToken ct);
    Task RegisterFoundReportAsync(string code, FoundReportRequest req, CancellationToken ct);
}