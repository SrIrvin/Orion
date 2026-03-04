using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;
using Orión.Domain.Entities;

using MaterialDesignThemes.Wpf;
using Orión.DesktopUI.Views;

namespace Orión.DesktopUI.ViewModels;

public partial class UsuarioViewModel : ObservableObject
{
    private readonly IUsuarioService _usuarioService;

    [ObservableProperty]
    private ObservableCollection<UsuarioDto> _usuarios = new();

    [ObservableProperty]
    private UsuarioDto? _selectedUsuario;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _searchText = string.Empty;

    private List<UsuarioDto> _allUsuarios = new();

    public UsuarioViewModel(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [RelayCommand]
    public async Task LoadUsuariosAsync()
    {
        IsBusy = true;
        try
        {
            var data = await _usuarioService.GetAllAsync();
            _allUsuarios = data.ToList();
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
            Usuarios = new ObservableCollection<UsuarioDto>(_allUsuarios);
        }
        else
        {
            var filtered = _allUsuarios.Where(u => 
                u.NombreUsuario.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || 
                (u.Email?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                u.Rol.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

            Usuarios = new ObservableCollection<UsuarioDto>(filtered);
        }
    }

    [RelayCommand]
    private async Task ToggleStatusAsync()
    {
        if (SelectedUsuario == null) return;

        IsBusy = true;
        try
        {
            await _usuarioService.ToggleStatusAsync(SelectedUsuario.IdUsuario);
            await LoadUsuariosAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CreateUsuarioAsync()
    {
        var viewModel = new UsuarioEditorViewModel(_usuarioService);
        var view = new UsuarioEditorView { DataContext = viewModel };

        await DialogHost.Show(view, "MainDialogHost");
        await LoadUsuariosAsync();
    }

    [RelayCommand]
    private async Task EditUsuarioAsync()
    {
        if (SelectedUsuario == null) return;

        var viewModel = new UsuarioEditorViewModel(_usuarioService, SelectedUsuario);
        var view = new UsuarioEditorView { DataContext = viewModel };

        await DialogHost.Show(view, "MainDialogHost");
        await LoadUsuariosAsync();
    }
}
