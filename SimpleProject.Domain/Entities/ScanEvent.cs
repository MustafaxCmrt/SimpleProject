using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleProject.Domain.Entities;

[Table("ScanEvent")]
public class ScanEvent : Entity
{
    [ForeignKey(nameof(Collar))]
    public int CollarId { get; set; }
    public Collar Collar { get; set; } = null!;

    [ForeignKey(nameof(Animal))]
    public int AnimalId { get; set; }
    public Animal Animal { get; set; } = null!;

    [Column(TypeName = "decimal(9,6)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(9,6)")]
    public decimal? Longitude { get; set; }

    public float? AccuracyMeters { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? Language { get; set; }
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
}