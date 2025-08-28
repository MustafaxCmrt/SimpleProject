namespace SimpleProject.Domain.Dtos.SubjectDtos;

public class SubjectRequest
{
    public string Type { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Notes { get; set; }
    public string? FotoUrl { get; set; }
}