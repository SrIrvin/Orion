using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Orión.Application.Interfaces;
using Orión.Domain.Entities;

namespace Orión.DesktopUI.ViewModels;

public record LoginSuccessMessage(Usuario User);

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly ISecureConfigService _secureConfigService;

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _rememberMe;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    public LoginViewModel(IAuthService authService, ISecureConfigService secureConfigService)
    {
        _authService = authService;
        _secureConfigService = secureConfigService;

        // Cargar estado previo de RememberMe
        var config = _secureConfigService.LoadConfig();
        RememberMe = config.RememberMe;
    }

    [RelayCommand]
    private async Task Login()
    {
        Console.WriteLine($"[DEBUG] Intento de login iniciado para usuario: {Username}");
        
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Ingrese usuario y contraseña.";
            return;
        }

        IsBusy = true;
        ErrorMessage = string.Empty;

        try
        {
            var user = await _authService.LoginAsync(Username, Password);
            if (user != null)
            {
                Console.WriteLine("[DEBUG] Autenticación exitosa. Procesando sesión...");
                // Manejar persistencia de sesión
                var config = _secureConfigService.LoadConfig();
                if (RememberMe)
                {
                    config.RememberMe = true;
                    config.LastUserId = user.IdUsuario;
                    config.SessionExpiry = DateTime.UtcNow.AddDays(7); // Expira en 7 días
                }
                else
                {
                    config.RememberMe = false;
                    config.LastUserId = null;
                    config.SessionExpiry = null;
                }
                _secureConfigService.SaveConfig(config);

                WeakReferenceMessenger.Default.Send(new LoginSuccessMessage(user));
            }
            else
            {
                ErrorMessage = "Usuario o contraseña incorrectos, o cuenta no activa.";
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
