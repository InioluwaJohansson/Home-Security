public class GetTokenDto : BaseResponse
{
    public string TokenCode { get; set; }
    public DateTime ExpiryTime { get; set; }
}