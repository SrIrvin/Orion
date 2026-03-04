using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Orión.Application.Interfaces;

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

    public DashboardViewModel(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [RelayCommand]
    public async Task LoadKpisAsync()
    {
        IsBusy = true;
        try
        {
            TotalMaquinaria = await _dashboardService.GetTotalMaquinariaAsync();
            TotalTecnicos = await _dashboardService.GetTotalTecnicosAsync();
            SolicitudesAbiertas = await _dashboardService.GetSolicitudesAbiertasAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }
}
