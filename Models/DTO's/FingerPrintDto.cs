namespace Home_Security.Models.DTOs;
public class CreateFingerPrintDto
{
    public BinaryData FingerPrintEncoding { get; set; }
}
public class GetFingerPrintDto
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public string FingerPrintId { get; set; }
}
public class FingerPrintResponseModel : BaseResponse
{
    public GetFingerPrintDto Data { get; set; }
}