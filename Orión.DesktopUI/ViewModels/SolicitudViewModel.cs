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

    [ObservableProperty]
    private string _searchText = string.Empty;

    private List<SolicitudServicioDto> _allSolicitudes = new();

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
            _allSolicitudes = data.ToList();
            ApplyFilter();

            var maqData = await _maquinariaService.GetAllAsync();
            Maquinas = new ObservableCollection<Maquinaria>(maqData);

            TiposMantenimiento = new ObservableCollection<TipoMantenimiento>(_context.TiposMantenimiento.ToList());
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            Solicitudes = new ObservableCollection<SolicitudServicioDto>(_allSolicitudes);
        }
        else
        {
            var filtered = _allSolicitudes.Where(s => 
                s.NombreMaquinaria.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || 
                (s.DescripcionFalla?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                s.EstadoDescripcion.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                s.IdSS.ToString().Contains(SearchText)).ToList();
            
            Solicitudes = new ObservableCollection<SolicitudServicioDto>(filtered);
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
