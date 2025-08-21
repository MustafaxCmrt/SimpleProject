using SimpleProject.Services.Interfaces;

namespace SimpleProject.Services;

public class NotificationService : INotificationService
{
    public Task NotifyOwnerOnScanAsync(int ownerUserId, int animalId, int scanEventId, CancellationToken ct) => Task.CompletedTask;
    public Task NotifyOwnerOnFoundAsync(int ownerUserId, int animalId, int foundReportId, CancellationToken ct) => Task.CompletedTask;
}