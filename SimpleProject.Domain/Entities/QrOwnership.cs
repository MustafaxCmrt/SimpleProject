using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleProject.Domain.Entities;

[Table("QrOwnership")]
public class QrOwnership
{
    [Key]
    public int Id { get; set; }
    [ForeignKey(nameof(Collar))]
    public int CollarId { get; set; }
    public Collar Collar { get; set; } = null!;
    [ForeignKey(nameof(OwnerUser))]
    public int OwnerUserId { get; set; }
    public AdminUser OwnerUser { get; set; } = null!;
    public DateTime ActivatedAt { get; set; }
    public bool IsActive { get; set; }
}