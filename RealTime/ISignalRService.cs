using Home_Security.Entities.Identity;

namespace Home_Security.RealTimeServices;
public interface IRealtimeNotificationService
{
     public Task NotifyAll<T>(string eventName, T payload);
     Task UpdateAppList(string eventName, Role role, object update);
}