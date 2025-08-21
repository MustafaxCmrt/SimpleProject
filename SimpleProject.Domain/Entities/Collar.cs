using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleProject.Domain.Entities;

[Table("Collar")]
public class Collar : Entity
{
    [Required, StringLength(64)]
    public string Code { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    [ForeignKey(nameof(Animal))]
    public int AnimalId { get; set; }
    public Animal Animal { get; set; } = null!;

    [ForeignKey(nameof(OwnerUser))]
    public int OwnerUserId { get; set; }
    public AdminUser OwnerUser { get; set; } = null!;
}