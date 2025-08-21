namespace SimpleProject.Domain.Dtos;

public class PetPublicDto
{
    public int AnimalId { get; set; }
    public string Ad { get; set; } = null!;
    public string Tur { get; set; } = null!;
    public string? Cinsiyet { get; set; }
    public bool SahipliMi { get; set; }
    public string? SahipAdMasked { get; set; }
    public string? SahipTelefonMasked { get; set; }
    public string? FotoUrl { get; set; }
}