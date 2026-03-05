using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;

using MaterialDesignThemes.Wpf;
using Orión.DesktopUI.Views;

namespace Orión.DesktopUI.ViewModels;

public partial class ComponenteViewModel : ObservableObject
{
    private readonly IComponenteService _componenteService;
    private readonly IProveedorService _proveedorService;
    private readonly IUserSessionService _sessionService;
    private readonly IOrionDbContext _context;

    [ObservableProperty]
    private ObservableCollection<ComponenteDto> _componentes = new();

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private bool _isAdmin;

    [ObservableProperty]
    private ComponenteDto? _selectedComponente;

    [ObservableProperty]
    private string _searchText = string.Empty;

    private List<ComponenteDto> _allComponentes = new();

    [ObservableProperty]
    private string _maquinariaId = string.Empty;

    public ComponenteViewModel(IComponenteService componenteService, IProveedorService proveedorService, IUserSessionService sessionService, IOrionDbContext context)
    {
        _componenteService = componenteService;
        _proveedorService = proveedorService;
        _sessionService = sessionService;
        _context = context;
        IsAdmin = _sessionService.IsAdmin;
    }

    [RelayCommand]
    public async Task LoadComponentesAsync()
    {
        if (string.IsNullOrEmpty(MaquinariaId)) return;

        IsBusy = true;
        try
        {
            var data = await _componenteService.GetByMaquinariaIdDtoAsync(MaquinariaId, includeInactive: IsAdmin);
            _allComponentes = data.ToList();
            ApplyFilter();
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
            Componentes = new ObservableCollection<ComponenteDto>(_allComponentes);
        }
        else
        {
            var filtered = _allComponentes.Where(c => 
                c.NombreComponente.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || 
                (c.Marca?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                c.IdComponente.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
            
            Componentes = new ObservableCollection<ComponenteDto>(filtered);
        }
    }

    [RelayCommand(CanExecute = nameof(IsAdmin))]
    private async Task CreateComponenteAsync()
    {
        if (string.IsNullOrEmpty(MaquinariaId)) return;

        var viewModel = new ComponenteEditorViewModel(_componenteService, _proveedorService, _context, MaquinariaId);
        await viewModel.InitializeAsync();
        
        var view = new ComponenteEditorView { DataContext = viewModel };

        await DialogHost.Show(view, "MainDialogHost");
        await LoadComponentesAsync();
    }

    [RelayCommand(CanExecute = nameof(CanEditOrDisable))]
    private async Task EditComponenteAsync()
    {
        if (SelectedComponente == null) return;

        var viewModel = new ComponenteEditorViewModel(_componenteService, _proveedorService, _context, MaquinariaId, SelectedComponente);
        await viewModel.InitializeAsync();

        var view = new ComponenteEditorView { DataContext = viewModel };

        await DialogHost.Show(view, "MainDialogHost");
        await LoadComponentesAsync();
    }

    [RelayCommand(CanExecute = nameof(CanEditOrDisable))]
    private async Task ToggleStatusAsync()
    {
        if (SelectedComponente == null) return;

        IsBusy = true;
        try
        {
            await _componenteService.ToggleStatusAsync(SelectedComponente.IdComponente);
            await LoadComponentesAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool CanEditOrDisable() => IsAdmin && SelectedComponente != null;

    partial void OnSelectedComponenteChanged(ComponenteDto? value)
    {
        EditComponenteCommand.NotifyCanExecuteChanged();
        ToggleStatusCommand.NotifyCanExecuteChanged();
    }
}
