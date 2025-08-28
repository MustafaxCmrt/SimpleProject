using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleProject.Domain.Entities;

[Table("Dealer")]
public class Dealer : Entity
{
    [Required, StringLength(50)]
    public string Code { get; set; } = null!;
    [Required, StringLength(150)]
    public string Name { get; set; } = null!;
    [StringLength(150)]
    public string? Contact { get; set; }
}