using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Orión.Application.Interfaces;
using Orión.DesktopUI.Views;
using Orión.DesktopUI.Interfaces;
using Orión.DesktopUI.Services;
using System.Diagnostics;

namespace Orión.DesktopUI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private bool _isAdmin;

    public object? CurrentView => _navigationService.CurrentView;

    public MainViewModel(INavigationService navigationService, IUserSessionService sessionService)
    {
        _navigationService = navigationService;
        ((NavigationService)_navigationService).PropertyChanged += (s, e) => 
        {
            if (e.PropertyName == nameof(INavigationService.CurrentView))
                OnPropertyChanged(nameof(CurrentView));
        };

        _navigationService.NavigateTo<DashboardView>();
        IsAdmin = sessionService.IsAdmin;
    }

    [RelayCommand]
    private void NavigateToDashboard() => _navigationService.NavigateTo<DashboardView>();

    [RelayCommand]
    private void NavigateToMaquinaria() => _navigationService.NavigateTo<MaquinariaListView>();

    [RelayCommand]
    private void NavigateToTecnicos() => _navigationService.NavigateTo<TecnicoListView>();

    [RelayCommand]
    private void NavigateToUsuarios()
    {
        if (IsAdmin) _navigationService.NavigateTo<UsuarioListView>();
    }

    [RelayCommand]
    private void NavigateToSolicitudes() => _navigationService.NavigateTo<SolicitudListView>();

    [RelayCommand]
    private void NavigateToComponentes(string maquinariaId)
    {
        _navigationService.NavigateTo<ComponenteListView>(view =>
        {
            if (view.DataContext is ComponenteViewModel vm)
            {
                vm.MaquinariaId = maquinariaId;
            }
        });
    }

    [RelayCommand]
    private void OpenEmail()
    {
        Process.Start(new ProcessStartInfo("mailto:sr._irvin@hotmail.com") { UseShellExecute = true });
    }

    [RelayCommand]
    private void OpenLinkedIn()
    {
        Process.Start(new ProcessStartInfo("https://www.linkedin.com/in/sr-irvin/") { UseShellExecute = true });
    }
}
