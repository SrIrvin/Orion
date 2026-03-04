using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;
using Orión.Domain.Entities;
using System.Collections.ObjectModel;

namespace Orión.DesktopUI.ViewModels;

public partial class MaquinariaEditorViewModel : ObservableObject
{
    private readonly IMaquinariaService _maquinariaService;
    private readonly IOrionDbContext _context;

    [ObservableProperty] private string _idMaquinaria = string.Empty;
    [ObservableProperty] private string _nombreMaquina = string.Empty;
    [ObservableProperty] private string? _tipo;
    [ObservableProperty] private string? _marca;
    [ObservableProperty] private string? _modelo;
    [ObservableProperty] private DateTime? _fechaInstalacion = DateTime.Today;
    [ObservableProperty] private int _idNivelCritico;
    [ObservableProperty] private int _idUbicacion;
    [ObservableProperty] private bool _isNew;
    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private ObservableCollection<NivelCritico> _nivelesCriticos = new();
    [ObservableProperty] private ObservableCollection<Ubicacion> _ubicaciones = new();

    public MaquinariaEditorViewModel(IMaquinariaService maquinariaService, IOrionDbContext context, MaquinariaDto? maquinaria = null)
    {
        _maquinariaService = maquinariaService;
        _context = context;
        
        LoadCatalogs();

        if (maquinaria == null)
        {
            IsNew = true;
            Title = "REGISTRAR MAQUINARIA";
            IdNivelCritico = 1;
            IdUbicacion = _context.Ubicaciones.FirstOrDefault()?.IdUbicacion ?? 0;
        }
        else
        {
            IsNew = false;
            Title = "EDITAR MAQUINARIA";
            IdMaquinaria = maquinaria.IdMaquinaria;
            NombreMaquina = maquinaria.NombreMaquina;
            Tipo = maquinaria.Tipo;
            Marca = maquinaria.Marca;
            Modelo = maquinaria.Modelo;
            FechaInstalacion = maquinaria.FechaInstalacion;
            IdNivelCritico = maquinaria.IdNivelCritico;
            IdUbicacion = maquinaria.IdUbicacion;
        }
    }

    private void LoadCatalogs()
    {
        NivelesCriticos = new ObservableCollection<NivelCritico>(_context.NivelesCriticos.ToList());
        Ubicaciones = new ObservableCollection<Ubicacion>(_context.Ubicaciones.ToList());
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(IdMaquinaria) || string.IsNullOrWhiteSpace(NombreMaquina)) return;

        try
        {
            if (IsNew)
            {
                await _maquinariaService.CreateAsync(new Maquinaria
                {
                    IdMaquinaria = IdMaquinaria,
                    NombreMaquina = NombreMaquina,
                    Tipo = Tipo,
                    Marca = Marca,
                    Modelo = Modelo,
                    FechaInstalacion = FechaInstalacion,
                    IdNivelCritico = IdNivelCritico,
                    IdUbicacion = IdUbicacion,
                    Activo = true
                });
            }
            else
            {
                var maq = await _maquinariaService.GetByIdAsync(IdMaquinaria);
                if (maq != null)
                {
                    maq.NombreMaquina = NombreMaquina;
                    maq.Tipo = Tipo;
                    maq.Marca = Marca;
                    maq.Modelo = Modelo;
                    maq.FechaInstalacion = FechaInstalacion;
                    maq.IdNivelCritico = IdNivelCritico;
                    maq.IdUbicacion = IdUbicacion;
                    await _maquinariaService.UpdateAsync(maq);
                }
            }

            DialogHost.Close("MainDialogHost");
        }
        catch (Exception ex)
        {
        }
    }
}
