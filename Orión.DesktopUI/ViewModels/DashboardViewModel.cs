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

    [ObservableProperty]
    private int _totalMaquinaria;

    [ObservableProperty]
    private int _totalTecnicos;

    [ObservableProperty]
    private int _solicitudesAbiertas;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private ObservableCollection<GlobalActivityHeatmapDto> _globalHeatmapData = new();

    public DashboardViewModel(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        try
        {
            await LoadKpisAsync();
            await LoadHeatmapAsync();
        }
        catch (Exception) { }
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
