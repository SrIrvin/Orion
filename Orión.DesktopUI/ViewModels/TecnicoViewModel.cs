using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;

using MaterialDesignThemes.Wpf;
using Orión.DesktopUI.Views;

namespace Orión.DesktopUI.ViewModels;

public partial class TecnicoViewModel : ObservableObject
{
    private readonly ITecnicoService _tecnicoService;
    private readonly IUserSessionService _sessionService;
    private readonly IOrionDbContext _context;

    [ObservableProperty]
    private ObservableCollection<TecnicoDto> _tecnicos = new();

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private bool _isAdmin;

    [ObservableProperty]
    private TecnicoDto? _selectedTecnico;

    public TecnicoViewModel(ITecnicoService tecnicoService, IUserSessionService sessionService, IOrionDbContext context)
    {
        _tecnicoService = tecnicoService;
        _sessionService = sessionService;
        _context = context;
        IsAdmin = _sessionService.IsAdmin;
    }

    [RelayCommand]
    public async Task LoadTecnicosAsync()
    {
        IsBusy = true;
        try
        {
            var data = await _tecnicoService.GetAllDtoAsync(includeInactive: IsAdmin);
            Tecnicos = new ObservableCollection<TecnicoDto>(data);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand(CanExecute = nameof(IsAdmin))]
    private async Task CreateTecnicoAsync()
    {
        var viewModel = new TecnicoEditorViewModel(_tecnicoService, _context);
        var view = new TecnicoEditorView { DataContext = viewModel };

        await DialogHost.Show(view, "MainDialogHost");
        await LoadTecnicosAsync();
    }

    [RelayCommand(CanExecute = nameof(CanEditOrDisable))]
    private async Task EditTecnicoAsync()
    {
        if (SelectedTecnico == null) return;

        var viewModel = new TecnicoEditorViewModel(_tecnicoService, _context, SelectedTecnico);
        var view = new TecnicoEditorView { DataContext = viewModel };

        await DialogHost.Show(view, "MainDialogHost");
        await LoadTecnicosAsync();
    }

    [RelayCommand(CanExecute = nameof(CanEditOrDisable))]
    private async Task ToggleStatusAsync()
    {
        if (SelectedTecnico == null) return;

        IsBusy = true;
        try
        {
            await _tecnicoService.ToggleStatusAsync(SelectedTecnico.IdPersonal);
            await LoadTecnicosAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool CanEditOrDisable() => IsAdmin && SelectedTecnico != null;

    partial void OnSelectedTecnicoChanged(TecnicoDto? value)
    {
        EditTecnicoCommand.NotifyCanExecuteChanged();
        ToggleStatusCommand.NotifyCanExecuteChanged();
    }
}
