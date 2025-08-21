using Microsoft.EntityFrameworkCore;
using SimpleProject.Data;
using SimpleProject.Domain.Dtos;
using SimpleProject.Services.Interfaces;

namespace SimpleProject.Services;

public class QrService : IQrService
{
    private readonly AppDbContext _db;
    private readonly INotificationService _notifier;

    public QrService(AppDbContext db, INotificationService notifier)
    {
        _db = db;
        _notifier = notifier;
    }

    public async Task<PetPublicDto> GetPetByCodeAsync(string code, CancellationToken ct)
    {
        var collar = await _db.Collars
            .Include(c => c.Animal)
            .FirstOrDefaultAsync(c => c.Code == code && c.IsActive && !c.Deleted, ct);

        if (collar == null || collar.Animal == null) throw new KeyNotFoundException("QR not found");

        var latestImage = await _db.PetImages
            .Where(pi => pi.AnimalId == collar.Animal.Id && !pi.Deleted)
            .OrderByDescending(pi => pi.CreateDate)
            .Select(pi => pi.Url)
            .FirstOrDefaultAsync(ct);

        return new PetPublicDto
        {
            AnimalId = collar.Animal.Id,
            Ad = collar.Animal.Ad,
            Tur = collar.Animal.Tur,
            Cinsiyet = collar.Animal.Cinsiyet,
            SahipliMi = collar.Animal.SahipliMi,
            SahipAdMasked = MaskName(collar.Animal.SahipAd),
            SahipTelefonMasked = MaskPhone(collar.Animal.SahipTelefon),
            FotoUrl = latestImage ?? collar.Animal.FotoUrl
        };
    }

    public async Task<ScanResponse> RegisterScanAsync(string code, ScanRequest req, string ip, string? userAgent, CancellationToken ct)
    {
        var collar = await _db.Collars
            .Include(c => c.Animal)
            .FirstOrDefaultAsync(c => c.Code == code && c.IsActive && !c.Deleted, ct);

        if (collar == null || collar.Animal == null) throw new KeyNotFoundException("QR not found");

        var scan = new Domain.Entities.ScanEvent
        {
            CollarId = collar.Id,
            AnimalId = collar.Animal.Id,
            Latitude = req.ConsentShareLocation ? req.Latitude : null,
            Longitude = req.ConsentShareLocation ? req.Longitude : null,
            AccuracyMeters = req.AccuracyMeters,
            IpAddress = ip,
            UserAgent = userAgent,
            Language = req.Language,
            OccurredAt = System.DateTime.UtcNow
        };

        _db.ScanEvents.Add(scan);
        await _db.SaveChangesAsync(ct);

        await _notifier.NotifyOwnerOnScanAsync(collar.OwnerUserId, collar.Animal.Id, scan.Id, ct);

        var petDto = await GetPetByCodeAsync(code, ct);

        var next = collar.Animal.SahipliMi
            ? new[] { "call_owner", "send_message", "create_found_report" }
            : new[] { "contact_optional" };

        return new ScanResponse
        {
            Pet = petDto,
            Status = "ok",
            NextActions = next,
            DeepLink = $"app://animal/{collar.Animal.Id}"
        };
    }

    public async Task RegisterFoundReportAsync(string code, FoundReportRequest req, CancellationToken ct)
    {
        var collar = await _db.Collars
            .Include(c => c.Animal)
            .FirstOrDefaultAsync(c => c.Code == code && c.IsActive && !c.Deleted, ct);

        if (collar == null || collar.Animal == null) throw new KeyNotFoundException("QR not found");

        var report = new Domain.Entities.FoundReport
        {
            AnimalId = collar.Animal.Id,
            CollarId = collar.Id,
            FullName = req.FullName,
            Phone = req.Phone,
            Message = req.Message,
            Latitude = req.Latitude,
            Longitude = req.Longitude
        };

        _db.FoundReports.Add(report);
        await _db.SaveChangesAsync(ct);

        await _notifier.NotifyOwnerOnFoundAsync(collar.OwnerUserId, collar.Animal.Id, report.Id, ct);
    }

    private static string? MaskPhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return null;
        var digits = new string(phone.Where(char.IsDigit).ToArray());
        if (digits.Length < 7) return phone;
        var head = digits[..3];
        var tail = digits[^2..];
        return $"{head}***{tail}";
    }

    private static string? MaskName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;
        if (name.Length <= 2) return name[..1] + "*";
        return name[..2] + "****";
    }
}