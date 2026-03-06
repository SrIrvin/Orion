using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Orión.Application.Interfaces;
using Orión.DesktopUI.Views;
using Orión.DesktopUI.Interfaces;
using Orión.DesktopUI.Services;
using System.Diagnostics;

using System.Windows;
using Microsoft.Extensions.Configuration;
using Orión.Application.DTOs;
using Microsoft.Win32;

namespace Orión.DesktopUI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly IUserSessionService _sessionService;
    private readonly IConfiguration _configuration;
    private readonly ISecureConfigService _secureConfigService;

    [ObservableProperty]
    private bool _isAdmin;

    [ObservableProperty]
    private string? _currentUserRole;

    [ObservableProperty]
    private DbConfigurationDto _configEditBuffer;

    [ObservableProperty]
    private bool _isTestingConnection;

    [ObservableProperty]
    private string? _testResultMessage;

    [ObservableProperty]
    private bool _isTestSuccessful;

    public object? CurrentView => _navigationService.CurrentView;

    public MainViewModel(
        INavigationService navigationService, 
        IUserSessionService sessionService, 
        IConfiguration configuration,
        ISecureConfigService secureConfigService)
    {
        _navigationService = navigationService;
        _sessionService = sessionService;
        _configuration = configuration;
        _secureConfigService = secureConfigService;

        _navigationService.CurrentViewChanged += () => OnPropertyChanged(nameof(CurrentView));

        _navigationService.NavigateTo<DashboardView>();
        IsAdmin = sessionService.IsAdmin;
        CurrentUserRole = sessionService.CurrentUser?.Rol ?? "Usuario";
        
        // Cargar configuración actual para edición
        ConfigEditBuffer = _secureConfigService.LoadConfig();

        // Registrar mensaje de navegación desde el mapa de calor global
        WeakReferenceMessenger.Default.Register<NavigateToSolicitudesMessage>(this, (r, m) =>
        {
            NavigateToSolicitudes();
        });
    }

    [RelayCommand]
    private async Task TestConnection()
    {
        IsTestingConnection = true;
        TestResultMessage = "Probando conexión...";
        IsTestSuccessful = false;

        bool success = await _secureConfigService.TestConnection(ConfigEditBuffer);

        IsTestingConnection = false;
        IsTestSuccessful = success;
        TestResultMessage = success ? "¡Conexión exitosa!" : "Error al conectar. Verifique los datos.";
    }

    [RelayCommand]
    private void SaveConfiguration()
    {
        _secureConfigService.SaveConfig(ConfigEditBuffer);
        MessageBox.Show("Configuración guardada exitosamente. La aplicación utilizará estos cambios en el próximo inicio.", "Configuración", MessageBoxButton.OK, MessageBoxImage.Information);
        
        // Cerrar el diálogo usando el ID global
        MaterialDesignThemes.Wpf.DialogHost.Close("MainDialogHost");
    }

    [RelayCommand]
    private void BrowseAccessFile()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Archivos de Access (*.accdb;*.mdb)|*.accdb;*.mdb|Todos los archivos (*.*)|*.*",
            Title = "Seleccionar Base de Datos Access"
        };

        if (dialog.ShowDialog() == true)
        {
            ConfigEditBuffer.AccessFilePath = dialog.FileName;
            OnPropertyChanged(nameof(ConfigEditBuffer));
        }
    }

    [RelayCommand]
    private void CopyConnectionString()
    {
        var conn = ConfigEditBuffer.GetConnectionString();
        if (!string.IsNullOrEmpty(conn))
        {
            Clipboard.SetText(conn);
        }
    }

    [RelayCommand]
    private void Logout()
    {
        _sessionService.CurrentUser = null;
        WeakReferenceMessenger.Default.Send(new LogoutMessage());
    }

    [RelayCommand]
    private void SwitchAccount()
    {
        _sessionService.CurrentUser = null;
        WeakReferenceMessenger.Default.Send(new LogoutMessage());
    }

    [RelayCommand]
    private void NavigateToDashboard() => _navigationService.NavigateTo<DashboardView>();

    [RelayCommand]
    private void NavigateToMaquinaria() => _navigationService.NavigateTo<MaquinariaListView>();

    [RelayCommand]
    private void NavigateToTecnicos() => _navigationService.NavigateTo<TecnicoListView>();

    [RelayCommand]
    private void NavigateToUsuarios()
    {
        if (IsAdmin) _navigationService.NavigateTo<UsuarioListView>();
    }

    [RelayCommand]
    private void NavigateToSolicitudes() => _navigationService.NavigateTo<SolicitudListView>();

    [RelayCommand]
    private void NavigateToProveedores() => _navigationService.NavigateTo<ProveedorListView>();

    [RelayCommand]
    private void NavigateToReportes() => _navigationService.NavigateTo<ReportView>();

    [RelayCommand]
    private void NavigateToComponentes(string maquinariaId)
    {
        _navigationService.NavigateTo<ComponenteListView>(view =>
        {
            if (view.DataContext is ComponenteViewModel vm)
            {
                vm.MaquinariaId = maquinariaId;
            }
        });
    }

    [RelayCommand]
    private void OpenEmail()
    {
        Process.Start(new ProcessStartInfo("mailto:sr._irvin@hotmail.com") { UseShellExecute = true });
    }

    [RelayCommand]
    private void OpenLinkedIn()
    {
        Process.Start(new ProcessStartInfo("https://www.linkedin.com/in/sr-irvin/") { UseShellExecute = true });
    }
}
