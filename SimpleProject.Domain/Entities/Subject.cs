using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleProject.Domain.Entities;

[Table("Subject")]
public class Subject : Entity
{
    [Required, StringLength(20)]
    public string Type { get; set; } = null!;
    [Required, StringLength(120)]
    public string Name { get; set; } = null!;
    [StringLength(1000)]
    public string? Notes { get; set; }
    [StringLength(500)]
    public string? FotoUrl { get; set; }
}