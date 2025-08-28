namespace SimpleProject.Domain.Dtos.SubjectDtos;

public class SubjectResponse
{
    public int Id { get; set; }
    public string Type { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Notes { get; set; }
    public string? FotoUrl { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}