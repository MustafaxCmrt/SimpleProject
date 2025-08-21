using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleProject.Domain.Entities;

[Table("Notification")]
public class Notification : Entity
{
    [ForeignKey(nameof(Animal))]
    public int HayvanId { get; set; }
    public Animal Hayvan { get; set; } = null!;

    [Required, StringLength(250)]
    public string Title { get; set; } = null!;

    [StringLength(1000)]
    public string? Message { get; set; }

    public bool IsRead { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}