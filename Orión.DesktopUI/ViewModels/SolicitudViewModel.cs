using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;
using Orión.Domain.Entities;

namespace Orión.DesktopUI.ViewModels;

public partial class SolicitudViewModel : ObservableObject
{
    private readonly ISolicitudServicioService _solicitudService;
    private readonly IMaquinariaService _maquinariaService;
    private readonly IReportService _reportService;
    private readonly IOrionDbContext _context;

    [ObservableProperty]
    private ObservableCollection<SolicitudServicioDto> _solicitudes = new();

    [ObservableProperty]
    private ObservableCollection<Maquinaria> _maquinas = new();

    [ObservableProperty]
    private ObservableCollection<TipoMantenimiento> _tiposMantenimiento = new();

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private bool _isFormVisible;

    // Campos para nueva solicitud
    [ObservableProperty]
    private string? _selectedMaquinariaId;

    [ObservableProperty]
    private int _selectedTipoManttoId;

    [ObservableProperty]
    private string _descripcionFalla = string.Empty;

    public SolicitudViewModel(
        ISolicitudServicioService solicitudService, 
        IMaquinariaService maquinariaService,
        IReportService reportService,
        IOrionDbContext context)
    {
        _solicitudService = solicitudService;
        _maquinariaService = maquinariaService;
        _reportService = reportService;
        _context = context;
    }

    [RelayCommand]
    public async Task LoadDataAsync()
    {
        IsBusy = true;
        try
        {
            var data = await _solicitudService.GetAllDtoAsync();
            Solicitudes = new ObservableCollection<SolicitudServicioDto>(data);

            var maqData = await _maquinariaService.GetAllAsync();
            Maquinas = new ObservableCollection<Maquinaria>(maqData);

            TiposMantenimiento = new ObservableCollection<TipoMantenimiento>(_context.TiposMantenimiento);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void ShowForm() => IsFormVisible = true;

    [RelayCommand]
    private void HideForm() => IsFormVisible = false;

    [RelayCommand]
    private async Task SaveSolicitudAsync()
    {
        if (string.IsNullOrEmpty(SelectedMaquinariaId) || SelectedTipoManttoId == 0) return;

        IsBusy = true;
        try
        {
            var nueva = new SolicitudServicio
            {
                IdMaquinaria = SelectedMaquinariaId,
                IdTipoMantto = SelectedTipoManttoId,
                DescripcionFalla = DescripcionFalla,
                IdEstadoSolicitud = 1 // Abierta
            };

            await _solicitudService.CreateAsync(nueva);
            await LoadDataAsync();
            IsFormVisible = false;
            DescripcionFalla = string.Empty;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void GenerateReport(SolicitudServicioDto solicitud)
    {
        var dialog = new SaveFileDialog
        {
            Filter = "Archivo PDF (*.pdf)|*.pdf",
            FileName = $"Orden_Servicio_{solicitud.IdSS:D5}.pdf"
        };

        if (dialog.ShowDialog() == true)
        {
            try
            {
                _reportService.GenerateSolicitudPdf(solicitud, dialog.FileName);
            }
            catch (Exception ex)
            {
                // El log global capturará errores más graves, aquí solo informamos
                System.Windows.MessageBox.Show($"Error al generar el PDF: {ex.Message}");
            }
        }
    }
}
