using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Orión.DesktopUI.Interfaces;

namespace Orión.DesktopUI.Services;

public partial class NavigationService : ObservableObject, INavigationService
{
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private object? _currentView;

    public event Action? CurrentViewChanged;

    partial void OnCurrentViewChanged(object? value)
    {
        CurrentViewChanged?.Invoke();
    }

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void NavigateTo<T>() where T : class
    {
        CurrentView = _serviceProvider.GetRequiredService<T>();
    }

    public void NavigateTo<T>(Action<T> parameterSetter) where T : class
    {
        var view = _serviceProvider.GetRequiredService<T>();
        parameterSetter(view);
        CurrentView = view;
    }
}
