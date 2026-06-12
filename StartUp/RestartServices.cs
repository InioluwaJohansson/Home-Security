namespace Home_Security.StartUp;

public class RestartService : IRestartService
{
    private readonly IHostApplicationLifetime _lifetime;

    public RestartService(IHostApplicationLifetime lifetime)
    {
        _lifetime = lifetime;
    }
    public void Restart()
    {
        _lifetime.StopApplication();
    }
}
public interface IRestartService
{
    void Restart();
}