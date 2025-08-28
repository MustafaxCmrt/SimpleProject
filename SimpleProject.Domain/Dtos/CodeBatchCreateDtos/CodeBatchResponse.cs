namespace SimpleProject.Domain.Dtos.CodeBatchCreateDtos;

public class CodeBatchResponse
{
    public int Id { get; set; }
    public int DealerId { get; set; }
    public string BatchCode { get; set; } = null!;
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
}