using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;
using Orión.Domain.Entities;
using System.Collections.ObjectModel;

namespace Orión.DesktopUI.ViewModels;

public partial class ComponenteEditorViewModel : ObservableObject
{
    private readonly IComponenteService _componenteService;
    private readonly IProveedorService _proveedorService;
    private readonly IOrionDbContext _context;

    [ObservableProperty] private string _idComponente = string.Empty;
    [ObservableProperty] private string _nombreComponente = string.Empty;
    [ObservableProperty] private string? _marca;
    [ObservableProperty] private string? _numeroSerie;
    [ObservableProperty] private string? _especificacionesTecnicas;
    [ObservableProperty] private DateTime? _fechaUltimoCambio = DateTime.Today;
    [ObservableProperty] private string _idMaquinaria = string.Empty;
    [ObservableProperty] private int _idTipoComponente;
    [ObservableProperty] private int _idEstado;
    [ObservableProperty] private string? _nombreProveedor;
    [ObservableProperty] private bool _isNew;
    [ObservableProperty] private string _title = string.Empty;
    
    [ObservableProperty] private ObservableCollection<TipoComponente> _tiposComponentes = new();
    [ObservableProperty] private ObservableCollection<EstadoComponente> _estadosComponentes = new();
    [ObservableProperty] private ObservableCollection<Proveedor> _proveedores = new();

    public ComponenteEditorViewModel(IComponenteService componenteService, IProveedorService proveedorService, IOrionDbContext context, string maquinariaId, ComponenteDto? componente = null)
    {
        _componenteService = componenteService;
        _proveedorService = proveedorService;
        _context = context;
        IdMaquinaria = maquinariaId;
        
        // Cargar catálogos sincrónicos
        TiposComponentes = new ObservableCollection<TipoComponente>(_context.TiposComponentes.ToList());
        EstadosComponentes = new ObservableCollection<EstadoComponente>(_context.EstadosComponentes.ToList());

        if (componente == null)
        {
            IsNew = true;
            Title = "REGISTRAR COMPONENTE";
            IdEstado = 1; // Activo
            IdTipoComponente = _context.TiposComponentes.FirstOrDefault()?.IdTipoComponente ?? 0;
        }
        else
        {
            IsNew = false;
            Title = "EDITAR COMPONENTE";
            IdComponente = componente.IdComponente;
            NombreComponente = componente.NombreComponente;
            Marca = componente.Marca;
            NumeroSerie = componente.NumeroSerie;
            EspecificacionesTecnicas = componente.EspecificacionesTecnicas;
            FechaUltimoCambio = componente.FechaUltimoCambio;
            IdTipoComponente = componente.IdTipoComponente;
            IdEstado = componente.IdEstado;
            NombreProveedor = componente.ProveedorNombre;
        }
    }

    public async Task InitializeAsync()
    {
        var provs = await _proveedorService.GetAllAsync();
        Proveedores = new ObservableCollection<Proveedor>(provs);
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(IdComponente) || string.IsNullOrWhiteSpace(NombreComponente)) return;

        try
        {
            Proveedor? proveedor = null;
            if (!string.IsNullOrWhiteSpace(NombreProveedor))
            {
                proveedor = await _proveedorService.GetOrCreateByNameAsync(NombreProveedor);
            }

            if (IsNew)
            {
                await _componenteService.CreateAsync(new Componente
                {
                    IdComponente = IdComponente,
                    IdMaquinaria = IdMaquinaria,
                    NombreComponente = NombreComponente,
                    Marca = Marca,
                    NumeroSerie = NumeroSerie,
                    EspecificacionesTecnicas = EspecificacionesTecnicas,
                    FechaUltimoCambio = FechaUltimoCambio,
                    IdTipoComponente = IdTipoComponente,
                    IdEstado = IdEstado,
                    IdProveedor = proveedor?.IdProveedor,
                    Activo = true
                });
            }
            else
            {
                var comp = await _componenteService.GetByIdAsync(IdComponente);
                if (comp != null)
                {
                    comp.NombreComponente = NombreComponente;
                    comp.Marca = Marca;
                    comp.NumeroSerie = NumeroSerie;
                    comp.EspecificacionesTecnicas = EspecificacionesTecnicas;
                    comp.FechaUltimoCambio = FechaUltimoCambio;
                    comp.IdTipoComponente = IdTipoComponente;
                    comp.IdEstado = IdEstado;
                    comp.IdProveedor = proveedor?.IdProveedor;
                    await _componenteService.UpdateAsync(comp);
                }
            }

            DialogHost.Close("MainDialogHost");
        }
        catch (Exception)
        {
        }
    }
}
