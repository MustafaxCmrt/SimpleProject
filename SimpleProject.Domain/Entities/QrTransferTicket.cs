using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleProject.Domain.Entities;

[Table("QrTransferTicket")]
public class QrTransferTicket
{
    [Key]
    public int Id { get; set; }
    [ForeignKey(nameof(Collar))]
    public int CollarId { get; set; }
    public Collar Collar { get; set; } = null!;
    [ForeignKey(nameof(FromOwnerUser))]
    public int? FromOwnerUserId { get; set; }
    public AdminUser? FromOwnerUser { get; set; }
    [ForeignKey(nameof(FromDealer))]
    public int? FromDealerId { get; set; }
    public Dealer? FromDealer { get; set; }
    [ForeignKey(nameof(ToOwnerUser))]
    public int? ToOwnerUserId { get; set; }
    public AdminUser? ToOwnerUser { get; set; }
    [Required, StringLength(20)]
    public string Status { get; set; } = null!;
    [Required, StringLength(64)]
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
}