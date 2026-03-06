using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;
using System.Collections.ObjectModel;

namespace Orión.DesktopUI.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly IDashboardService _dashboardService;
    private readonly IUserSessionService _sessionService;

    [ObservableProperty]
    private int _totalMaquinaria;

    [ObservableProperty]
    private int _totalTecnicos;

    [ObservableProperty]
    private int _solicitudesAbiertas;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private bool _showSecurityWarning;

    [ObservableProperty]
    private ObservableCollection<GlobalActivityHeatmapDto> _globalHeatmapData = new();

    public DashboardViewModel(IDashboardService dashboardService, IUserSessionService sessionService)
    {
        _dashboardService = dashboardService;
        _sessionService = sessionService;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        try
        {
            CheckSecurityStatus();
            await LoadKpisAsync();
            await LoadHeatmapAsync();
        }
        catch (Exception) { }
    }

    private void CheckSecurityStatus()
    {
        var user = _sessionService.CurrentUser;
        if (user != null && user.NombreUsuario == "admin" && user.RequiresPasswordChange)
        {
            ShowSecurityWarning = true;
        }
    }

    [RelayCommand]
    private void DismissSecurityWarning()
    {
        ShowSecurityWarning = false;
    }

    [RelayCommand]
    private void NavigateToUserProfile()
    {
        // En el futuro, navegar al perfil. Por ahora solo cerramos el warning.
        ShowSecurityWarning = false;
        // Podríamos disparar un mensaje de navegación si existiera la vista de perfil
    }

    [RelayCommand]
    public async Task LoadKpisAsync()
    {
        try
        {
            TotalMaquinaria = await _dashboardService.GetTotalMaquinariaAsync();
            TotalTecnicos = await _dashboardService.GetTotalTecnicosAsync();
            SolicitudesAbiertas = await _dashboardService.GetSolicitudesAbiertasAsync();
        }
        catch (Exception) { }
    }

    [RelayCommand]
    public async Task LoadHeatmapAsync()
    {
        IsBusy = true;
        try
        {
            var data = await _dashboardService.GetGlobalActivityHeatmapAsync();
            GlobalHeatmapData = new ObservableCollection<GlobalActivityHeatmapDto>(data);
        }
        catch (Exception) { }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void NavigateToSolicitudes()
    {
        // Notificar navegación general a solicitudes
        WeakReferenceMessenger.Default.Send(new NavigateToSolicitudesMessage());
    }
}

public record NavigateToSolicitudesMessage();
