namespace SimpleProject.Domain.Dtos;

public class FoundReportRequest : EntityDto
{
    public string FullName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Message { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}