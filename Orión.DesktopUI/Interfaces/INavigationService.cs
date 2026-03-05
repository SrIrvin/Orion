namespace Orión.DesktopUI.Interfaces;

public interface INavigationService
{
    object? CurrentView { get; }
    void NavigateTo<T>() where T : class;
    void NavigateTo<T>(Action<T> parameterSetter) where T : class;
}
