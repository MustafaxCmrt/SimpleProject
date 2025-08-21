namespace SimpleProject.Services.Interfaces;

public interface INotificationService
{
    Task NotifyOwnerOnScanAsync(int ownerUserId, int animalId, int scanEventId, CancellationToken ct);
    Task NotifyOwnerOnFoundAsync(int ownerUserId, int animalId, int foundReportId, CancellationToken ct);
}