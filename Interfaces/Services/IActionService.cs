namespace Home_Security.Interfaces.Services;
public interface IActionService
{
    public Task TriggerActionsForExternal(int externalId);
}