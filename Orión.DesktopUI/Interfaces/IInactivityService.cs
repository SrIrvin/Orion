namespace Orión.DesktopUI.Interfaces;

public interface IInactivityService
{
    void StartMonitoring();
    void StopMonitoring();
    event Action OnInactivityTimeout;
}
