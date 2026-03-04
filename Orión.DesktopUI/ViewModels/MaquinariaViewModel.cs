using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;

using MaterialDesignThemes.Wpf;
using Orión.DesktopUI.Views;

namespace Orión.DesktopUI.ViewModels;

public partial class MaquinariaViewModel : ObservableObject
{
    private readonly IMaquinariaService _maquinariaService;
    private readonly IUserSessionService _sessionService;
    private readonly IOrionDbContext _context;

    [ObservableProperty]
    private ObservableCollection<MaquinariaDto> _maquinarias = new();

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private bool _isAdmin;

    [ObservableProperty]
    private MaquinariaDto? _selectedMaquinaria;

    public MaquinariaViewModel(IMaquinariaService maquinariaService, IUserSessionService sessionService, IOrionDbContext context)
    {
        _maquinariaService = maquinariaService;
        _sessionService = sessionService;
        _context = context;
        IsAdmin = _sessionService.IsAdmin;
    }

    [RelayCommand]
    public async Task LoadMaquinariasAsync()
    {
        IsBusy = true;
        try
        {
            var data = await _maquinariaService.GetAllDtoAsync(includeInactive: IsAdmin);
            Maquinarias = new ObservableCollection<MaquinariaDto>(data);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand(CanExecute = nameof(IsAdmin))]
    private async Task CreateMaquinariaAsync()
    {
        var viewModel = new MaquinariaEditorViewModel(_maquinariaService, _context);
        var view = new MaquinariaEditorView { DataContext = viewModel };

        await DialogHost.Show(view, "MainDialogHost");
        await LoadMaquinariasAsync();
    }

    [RelayCommand(CanExecute = nameof(CanEditOrDisable))]
    private async Task EditMaquinariaAsync()
    {
        if (SelectedMaquinaria == null) return;

        var viewModel = new MaquinariaEditorViewModel(_maquinariaService, _context, SelectedMaquinaria);
        var view = new MaquinariaEditorView { DataContext = viewModel };

        await DialogHost.Show(view, "MainDialogHost");
        await LoadMaquinariasAsync();
    }

    [RelayCommand(CanExecute = nameof(CanEditOrDisable))]
    private async Task ToggleStatusAsync()
    {
        if (SelectedMaquinaria == null) return;

        IsBusy = true;
        try
        {
            await _maquinariaService.ToggleStatusAsync(SelectedMaquinaria.IdMaquinaria);
            await LoadMaquinariasAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool CanEditOrDisable() => IsAdmin && SelectedMaquinaria != null;

    partial void OnSelectedMaquinariaChanged(MaquinariaDto? value)
    {
        EditMaquinariaCommand.NotifyCanExecuteChanged();
        ToggleStatusCommand.NotifyCanExecuteChanged();
    }
}
