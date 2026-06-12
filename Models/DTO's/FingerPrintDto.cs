namespace Home_Security.Models.DTOs;
public class CreateFingerPrintDto
{
    public int PersonId { get; set; }
    public List<byte []> FingerPrintEncoding { get; set; }
}
public class GetFingerPrintDto
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public string FingerPrintId { get; set; }
}
public class FingerPrintDataDto
{
    public string Token { get; set; }
    public int PersonId { get; set; }
    public byte [] FingerPrintEncoding { get; set; }
}
public class FingerPrintPayload
{
    public int PersonId { get; set; }
    public string Message { get; set; }
}
public class FingerPrintVerifyDataDto
{
    public byte [] Data { get; set; }
    public string Token { get; set; }
    public string HardwareId { get; set;}
}
public class FingerPrintResponseModel : BaseResponse
{
    public GetFingerPrintDto Data { get; set; }
}