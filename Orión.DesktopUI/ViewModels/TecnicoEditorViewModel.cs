using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;
using Orión.Domain.Entities;
using System.Collections.ObjectModel;

namespace Orión.DesktopUI.ViewModels;

public partial class TecnicoEditorViewModel : ObservableObject
{
    private readonly ITecnicoService _tecnicoService;
    private readonly IOrionDbContext _context;

    [ObservableProperty] private int _idPersonal;
    [ObservableProperty] private string _nombreApellido = string.Empty;
    [ObservableProperty] private string? _especialidad;
    [ObservableProperty] private int _idTurno;
    [ObservableProperty] private bool _isNew;
    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private ObservableCollection<Turno> _turnos = new();

    public TecnicoEditorViewModel(ITecnicoService tecnicoService, IOrionDbContext context, TecnicoDto? tecnico = null)
    {
        _tecnicoService = tecnicoService;
        _context = context;
        
        LoadTurnos();

        if (tecnico == null)
        {
            IsNew = true;
            Title = "REGISTRAR TÉCNICO";
            IdTurno = 1; // Por defecto Matutino
        }
        else
        {
            IsNew = false;
            Title = "EDITAR TÉCNICO";
            IdPersonal = tecnico.IdPersonal;
            NombreApellido = tecnico.NombreApellido;
            Especialidad = tecnico.Especialidad;
            IdTurno = tecnico.IdTurno;
        }
    }

    private void LoadTurnos()
    {
        var turnosList = _context.Turnos.ToList();
        Turnos = new ObservableCollection<Turno>(turnosList);
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(NombreApellido)) return;

        try
        {
            if (IsNew)
            {
                await _tecnicoService.CreateAsync(new Tecnico
                {
                    IdPersonal = IdPersonal,
                    NombreApellido = NombreApellido,
                    Especialidad = Especialidad,
                    IdTurno = IdTurno,
                    Activo = true
                });
            }
            else
            {
                var tecnico = await _tecnicoService.GetByIdAsync(IdPersonal);
                if (tecnico != null)
                {
                    tecnico.NombreApellido = NombreApellido;
                    tecnico.Especialidad = Especialidad;
                    tecnico.IdTurno = IdTurno;
                    await _tecnicoService.UpdateAsync(tecnico);
                }
            }

            DialogHost.Close("MainDialogHost");
        }
        catch (Exception)
        {
            // Manejo de errores simplificado
        }
    }
}
