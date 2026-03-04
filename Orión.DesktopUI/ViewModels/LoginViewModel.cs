using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Orión.Application.Interfaces;
using Orión.Domain.Entities;

namespace Orión.DesktopUI.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService _authService;

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private bool _isBusy;

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Usuario y contraseña requeridos.";
            return;
        }

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var user = await _authService.LoginAsync(Username, Password);

            if (user != null)
            {
                // Aquí se manejaría la navegación al Dashboard
                ErrorMessage = $"¡Bienvenido {user.NombreUsuario}!";
            }
            else
            {
                ErrorMessage = "Credenciales incorrectas o usuario inactivo.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
