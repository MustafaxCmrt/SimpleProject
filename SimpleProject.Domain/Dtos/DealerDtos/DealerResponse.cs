namespace SimpleProject.Domain.Dtos.DealerDtos;

public class DealerResponse
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Contact { get; set; }
}