namespace SimpleProject.Domain.Dtos.CodeBatchCreateDtos;

public class CodeBatchCreateRequest
{
    public int DealerId { get; set; }
    public string BatchCode { get; set; } = null!;
    public int Quantity { get; set; }
}