using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;
using Orión.Domain.Entities;

namespace Orión.DesktopUI.ViewModels;

public partial class UsuarioEditorViewModel : ObservableObject
{
    private readonly IUsuarioService _usuarioService;

    [ObservableProperty] private string _nombreUsuario = string.Empty;
    [ObservableProperty] private string? _email;
    [ObservableProperty] private string _rol = "Operador";
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private bool _isNew;
    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private int _idUsuario;

    public UsuarioEditorViewModel(IUsuarioService usuarioService, UsuarioDto? usuario = null)
    {
        _usuarioService = usuarioService;

        if (usuario == null)
        {
            IsNew = true;
            Title = "NUEVO USUARIO";
        }
        else
        {
            IsNew = false;
            Title = "EDITAR USUARIO";
            IdUsuario = usuario.IdUsuario;
            NombreUsuario = usuario.NombreUsuario;
            Email = usuario.Email;
            Rol = usuario.Rol;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(NombreUsuario)) return;

        try
        {
            if (IsNew)
            {
                if (string.IsNullOrWhiteSpace(Password)) return;

                await _usuarioService.CreateAsync(new Usuario
                {
                    NombreUsuario = NombreUsuario,
                    Email = Email,
                    Rol = Rol
                }, Password);
            }
            else
            {
                var usuario = await _usuarioService.GetByIdAsync(IdUsuario);
                if (usuario != null)
                {
                    usuario.Email = Email;
                    usuario.Rol = Rol;
                    await _usuarioService.UpdateAsync(usuario);
                }
            }

            DialogHost.Close("MainDialogHost");
        }
        catch (Exception ex)
        {
            // Error handling logic would go here
        }
    }
}
