using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Orión.DesktopUI.Views;

namespace Orión.DesktopUI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private object? _currentView;

    public MainViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _currentView = _serviceProvider.GetRequiredService<DashboardView>();
    }

    [RelayCommand]
    private void NavigateToDashboard()
    {
        CurrentView = _serviceProvider.GetRequiredService<DashboardView>();
    }

    [RelayCommand]
    private void NavigateToMaquinaria()
    {
        CurrentView = _serviceProvider.GetRequiredService<MaquinariaListView>();
    }

    [RelayCommand]
    private void NavigateToTecnicos()
    {
        CurrentView = _serviceProvider.GetRequiredService<TecnicoListView>();
    }

    [RelayCommand]
    private void NavigateToSolicitudes()
    {
        CurrentView = _serviceProvider.GetRequiredService<SolicitudListView>();
    }

    [RelayCommand]
    private void NavigateToComponentes(string maquinariaId)
    {
        var view = _serviceProvider.GetRequiredService<ComponenteListView>();
        if (view.DataContext is ComponenteViewModel vm)
        {
            vm.MaquinariaId = maquinariaId;
        }
        CurrentView = view;
    }
}
