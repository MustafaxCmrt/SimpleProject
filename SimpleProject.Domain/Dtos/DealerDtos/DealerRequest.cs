namespace SimpleProject.Domain.Dtos.DealerDtos;

public class DealerRequest
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Contact { get; set; }
}