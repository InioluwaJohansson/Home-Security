using Home_Security.Contracts;
using Home_Security.Models.Enums;
namespace Home_Security.Entities.Identity;
public class User : AuditableEntity
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string AuthorizationCode { get; set; }
    public string NFID  { get; set; }
    public string NFIDCode { get; set; }
    public UserRole UserRole { get; set; }
}
