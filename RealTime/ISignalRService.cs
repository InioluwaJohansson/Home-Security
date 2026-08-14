using Home_Security.Entities.Identity;

namespace Home_Security.RealTimeServices;
public interface IRealtimeNotificationService
{
     public Task NotifyAll<T>(string eventName, T payload);
     public Task NotifyAll<T>(string eventName, int personId, T payload);
     public Task NotifyAll<T>(string eventName, List<Role> roles, T payload);
     public Task UpdateAppList(string eventName, Role role, object update);
}