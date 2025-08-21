namespace SimpleProject.Domain.Dtos;

public class ScanResponse
{
    public PetPublicDto Pet { get; set; } = null!;
    public string Status { get; set; } = "ok";
    public string[] NextActions { get; set; } = System.Array.Empty<string>();
    public string? DeepLink { get; set; }
}