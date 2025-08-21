namespace SimpleProject.Domain.Dtos;

public class ScanRequest : EntityDto
{
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public float? AccuracyMeters { get; set; }
    public string? Device { get; set; }
    public string? Language { get; set; }
    public bool ConsentShareLocation { get; set; } = false;
}