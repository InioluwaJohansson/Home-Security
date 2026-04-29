using Home_Security.Models.DTOs;
namespace Home_Security.Models.DTOs;
public class GetTokenDto : BaseResponse
{
    public string TokenCode { get; set; }
    public DateTime ExpiryTime { get; set; }
}