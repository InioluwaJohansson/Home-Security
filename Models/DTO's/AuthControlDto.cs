using Home_Security.Entities.Identity;

namespace Home_Security.Models.DTOs;
public class GetAuthControlDto : BaseResponse
{
    public int Id { get; set; }
    public Role Role { get; set; }
    public bool IsDisabled { get; set; }
}
public class GetAuthControlInfoDto
{
    public int PersonId { get; set; }
    public string AuthorizationCode { get; set; }
}