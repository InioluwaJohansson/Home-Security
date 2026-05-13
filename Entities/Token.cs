using Home_Security.Contracts;
namespace Home_Security.Entities;
public class Token : AuditableEntity
{
    public string TokenCode { get; set; }
    //Token For ESP32 communication and for User Authorization password and auth code resets
    public DateTime ExpiryTime { get; set; }
}