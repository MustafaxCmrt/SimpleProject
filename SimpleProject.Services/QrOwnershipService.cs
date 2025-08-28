using Microsoft.EntityFrameworkCore;
using SimpleProject.Data;
using SimpleProject.Domain.Dtos.QrClaimDto;
using SimpleProject.Domain.Dtos.QROwnershipDto;
using SimpleProject.Services.Interfaces;

namespace SimpleProject.Services;

public class QrOwnershipService : IQrOwnershipService
{
    private readonly AppDbContext _db;

    public QrOwnershipService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<QrOwnershipResponse> ClaimAsync(QrClaimRequest req, CancellationToken ct)
    {
        var collar = await _db.Collars.FirstOrDefaultAsync(x => x.Code == req.Code, ct);
        if (collar == null) throw new InvalidOperationException("QR kod bulunamadı");

        var userExists = await _db.Set<Domain.Entities.AdminUser>().AnyAsync(x => x.Id == req.ToOwnerUserId, ct);
        if (!userExists) throw new InvalidOperationException("Sahip bulunamadı");

        var existingActive = await _db.QrOwnerships
            .FirstOrDefaultAsync(x => x.CollarId == collar.Id && x.IsActive, ct);
        if (existingActive != null) existingActive.IsActive = false;

        var now = DateTime.UtcNow;
        var ownership = new Domain.Entities.QrOwnership
        {
            CollarId = collar.Id,
            OwnerUserId = req.ToOwnerUserId,
            ActivatedAt = now,
            IsActive = true
        };
        _db.QrOwnerships.Add(ownership);

        collar.OwnerUserId = req.ToOwnerUserId;
        if (!string.IsNullOrWhiteSpace(req.FriendlyName)) collar.FriendlyName = req.FriendlyName;
        if (!string.IsNullOrWhiteSpace(req.AssetType)) collar.AssetType = req.AssetType!;
        if (req.SubjectId.HasValue) collar.SubjectId = req.SubjectId;

        await _db.SaveChangesAsync(ct);

        return new QrOwnershipResponse
        {
            OwnershipId = ownership.Id,
            CollarId = collar.Id,
            OwnerUserId = req.ToOwnerUserId,
            ActivatedAt = now,
            IsActive = true
        };
    }
}