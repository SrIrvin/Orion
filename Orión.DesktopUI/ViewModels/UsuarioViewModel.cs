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
            Usuarios = new ObservableCollection<UsuarioDto>(data);
        }
        finally
        {
            IsBusy = false;
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
