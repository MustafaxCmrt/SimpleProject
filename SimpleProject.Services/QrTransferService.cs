using Microsoft.EntityFrameworkCore;
using SimpleProject.Data;
using SimpleProject.Domain.Dtos.QROwnershipDto;
using SimpleProject.Domain.Dtos.QRTransferDto;
using SimpleProject.Services.Interfaces;

namespace SimpleProject.Services;

public class QrTransferService : IQrTransferService
{
    private readonly AppDbContext _db;

    public QrTransferService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<QrTransferInitResponse> InitAsync(QrTransferInitRequest req, CancellationToken ct)
    {
        var collar = await _db.Collars.FirstOrDefaultAsync(x => x.Code == req.Code, ct);
        if (collar == null) throw new InvalidOperationException("QR kod bulunamadı");

        if (req.ToOwnerUserId.HasValue)
        {
            var toExists = await _db.Set<Domain.Entities.AdminUser>().AnyAsync(x => x.Id == req.ToOwnerUserId, ct);
            if (!toExists) throw new InvalidOperationException("Hedef kullanıcı bulunamadı");
        }

        var token = Convert.ToHexString(Guid.NewGuid().ToByteArray()).ToLowerInvariant();
        var expiresAt = DateTime.UtcNow.AddMinutes(Math.Max(1, req.ExpiresInMinutes));

        var ticket = new Domain.Entities.QrTransferTicket
        {
            CollarId = collar.Id,
            FromOwnerUserId = req.FromOwnerUserId,
            FromDealerId = req.FromDealerId,
            ToOwnerUserId = req.ToOwnerUserId,
            Status = "PENDING",
            Token = token,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow
        };

        _db.QrTransferTickets.Add(ticket);
        await _db.SaveChangesAsync(ct);

        return new QrTransferInitResponse
        {
            TicketId = ticket.Id,
            Token = token,
            ExpiresAt = expiresAt,
            Status = "PENDING"
        };
    }

    public async Task<QrOwnershipResponse> AcceptAsync(QrTransferAcceptRequest req, CancellationToken ct)
    {
        var ticket = await _db.QrTransferTickets
            .Include(t => t.Collar)
            .FirstOrDefaultAsync(t => t.Token == req.Token, ct);

        if (ticket == null) throw new InvalidOperationException("Transfer tokeni bulunamadı");
        if (ticket.Status != "PENDING") throw new InvalidOperationException("Transfer pending yok");
        if (ticket.ExpiresAt <= DateTime.UtcNow) throw new InvalidOperationException("Transfer tokeni günü geçmiş");

        var userExists = await _db.Set<Domain.Entities.AdminUser>().AnyAsync(x => x.Id == req.ToOwnerUserId, ct);
        if (!userExists) throw new InvalidOperationException("Hedef kullanıcı bulunamadı");

        var prev = await _db.QrOwnerships
            .FirstOrDefaultAsync(x => x.CollarId == ticket.CollarId && x.IsActive, ct);
        if (prev != null) prev.IsActive = false;

        var now = DateTime.UtcNow;
        var newOwn = new Domain.Entities.QrOwnership
        {
            CollarId = ticket.CollarId,
            OwnerUserId = req.ToOwnerUserId,
            ActivatedAt = now,
            IsActive = true
        };
        _db.QrOwnerships.Add(newOwn);

        ticket.Status = "ACCEPTED";
        ticket.ToOwnerUserId = req.ToOwnerUserId;

        ticket.Collar.OwnerUserId = req.ToOwnerUserId;

        await _db.SaveChangesAsync(ct);

        return new QrOwnershipResponse
        {
            OwnershipId = newOwn.Id,
            CollarId = newOwn.CollarId,
            OwnerUserId = newOwn.OwnerUserId,
            ActivatedAt = now,
            IsActive = true
        };
    }
}