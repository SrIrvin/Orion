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

namespace Orión.DesktopUI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly IUserSessionService _sessionService;
    private readonly IConfiguration _configuration;

    [ObservableProperty]
    private bool _isAdmin;

    [ObservableProperty]
    private string? _currentUserRole;

    [ObservableProperty]
    private string? _currentDbProvider;

    [ObservableProperty]
    private string? _currentConnectionString;

    public object? CurrentView => _navigationService.CurrentView;

    public MainViewModel(INavigationService navigationService, IUserSessionService sessionService, IConfiguration configuration)
    {
        _navigationService = navigationService;
        _sessionService = sessionService;
        _configuration = configuration;

        _navigationService.CurrentViewChanged += () => OnPropertyChanged(nameof(CurrentView));

        _navigationService.NavigateTo<DashboardView>();
        IsAdmin = sessionService.IsAdmin;
        CurrentUserRole = sessionService.CurrentUser?.Rol ?? "Usuario";
        
        // Cargar info de DB
        CurrentDbProvider = _configuration.GetValue<string>("DbProvider") ?? "PostgreSQL";
        var connStringName = CurrentDbProvider.Equals("Access", StringComparison.OrdinalIgnoreCase) 
            ? "AccessConnection" 
            : (_configuration.GetValue<string>("Environment") == "Staging" ? "StagingConnection" : "DefaultConnection");
        
        var rawConn = _configuration.GetConnectionString(connStringName);
        if (CurrentDbProvider.Equals("Access", StringComparison.OrdinalIgnoreCase))
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            CurrentConnectionString = rawConn?.Replace("{Documents}", documentsPath);
        }
        else
        {
            CurrentConnectionString = rawConn;
        }

        // Registrar mensaje de navegación desde el mapa de calor global
        WeakReferenceMessenger.Default.Register<NavigateToSolicitudesMessage>(this, (r, m) =>
        {
            NavigateToSolicitudes();
        });
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
    private void CopyConnectionString()
    {
        if (!string.IsNullOrEmpty(CurrentConnectionString))
        {
            Clipboard.SetText(CurrentConnectionString);
            // Podríamos enviar un mensaje al Snackbar aquí
        }
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
