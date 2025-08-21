using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleProject.Domain.Entities;

[Table("FoundReport")]
public class FoundReport : Entity
{
    [ForeignKey(nameof(Collar))]
    public int CollarId { get; set; }
    public Collar Collar { get; set; } = null!;

    [ForeignKey(nameof(Animal))]
    public int AnimalId { get; set; }
    public Animal Animal { get; set; } = null!;

    [Required, StringLength(120)]
    public string FullName { get; set; } = null!;

    [Required, StringLength(30)]
    public string Phone { get; set; } = null!;

    [StringLength(1000)]
    public string? Message { get; set; }

    [Column(TypeName = "decimal(9,6)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(9,6)")]
    public decimal? Longitude { get; set; }
}