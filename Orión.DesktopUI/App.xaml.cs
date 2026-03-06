using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CommunityToolkit.Mvvm.Messaging;
using Orión.Application.Interfaces;
using Orión.Application.Services;
using Orión.DesktopUI.ViewModels;
using Orión.DesktopUI.Views;
using Orión.DesktopUI.Interfaces;
using Orión.DesktopUI.Services;
using Orión.Infrastructure.Persistence;
using Orión.Infrastructure.Repositories;
using Orión.Infrastructure.Services;

using Orión.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Orión.DesktopUI;

public partial class App : System.Windows.Application
{
    private readonly IHost _host;
    private LoginView? _loginView;
    private static readonly string LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");

    public App()
    {
        // ... (resto del constructor igual hasta ConfigureServices)
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration;
                var environment = configuration.GetValue<string>("Environment") ?? "Development";

                // Persistencia (Encapsulado en Infrastructure siguiendo SOLID)
                services.AddOrionPersistence(configuration, environment);

                // Navegación UI
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<IInactivityService, InactivityService>();

                // Servicios de Aplicación
                services.AddSingleton<IUserSessionService, UserSessionService>();
                services.AddScoped<IAuthService, AuthService>();
                services.AddScoped<IMaquinariaService, MaquinariaService>();
                services.AddScoped<IComponenteService, ComponenteService>();
                services.AddScoped<ITecnicoService, TecnicoService>();
                services.AddScoped<IUsuarioService, UsuarioService>();
                services.AddScoped<ISolicitudServicioService, SolicitudServicioService>();
                services.AddScoped<IProveedorService, ProveedorService>();
                services.AddScoped<IReportService, ReportService>();
                services.AddScoped<IDashboardService, DashboardService>();

                // ViewModels
                services.AddTransient<LoginViewModel>();
                services.AddTransient<MainViewModel>();
                services.AddTransient<MaquinariaViewModel>();
                services.AddTransient<ComponenteViewModel>();
                services.AddTransient<TecnicoViewModel>();
                services.AddTransient<UsuarioViewModel>();
                services.AddTransient<SolicitudViewModel>();
                services.AddTransient<ProveedorViewModel>();
                services.AddTransient<ReportViewModel>();
                services.AddTransient<DashboardViewModel>();

                // Views
                services.AddTransient<LoginView>();
                services.AddTransient<MainView>();
                services.AddTransient<DashboardView>();
                services.AddTransient<MaquinariaListView>();
                services.AddTransient<ComponenteListView>();
                services.AddTransient<TecnicoListView>();
                services.AddTransient<UsuarioListView>();
                services.AddTransient<SolicitudListView>();
                services.AddTransient<ProveedorListView>();
                services.AddTransient<ReportView>();
            })
            .Build();

        // Suscribirse al mensaje de login
        WeakReferenceMessenger.Default.Register<LoginSuccessMessage>(this, (r, m) =>
        {
            // Guardar usuario en la sesión
            var session = _host.Services.GetRequiredService<IUserSessionService>();
            session.CurrentUser = m.User;

            // Iniciar monitoreo de inactividad
            var inactivityService = _host.Services.GetRequiredService<IInactivityService>();
            inactivityService.StartMonitoring();

            var mainView = _host.Services.GetRequiredService<MainView>();
            mainView.Show();
            _loginView?.Close();
        });

        // Suscribirse al mensaje de logout
        WeakReferenceMessenger.Default.Register<LogoutMessage>(this, (r, m) =>
        {
            // Detener monitoreo
            var inactivityService = _host.Services.GetRequiredService<IInactivityService>();
            inactivityService.StopMonitoring();

            // Reabrir Login
            _loginView = _host.Services.GetRequiredService<LoginView>();
            _loginView.Show();

            // Cerrar la ventana principal activa
            var activeMainView = System.Windows.Application.Current.Windows.OfType<MainView>().FirstOrDefault();
            activeMainView?.Close();
        });

        // Manejar tiempo de inactividad
        var globalInactivity = _host.Services.GetRequiredService<IInactivityService>();
        globalInactivity.OnInactivityTimeout += () =>
        {
            Dispatcher.Invoke(() =>
            {
                var session = _host.Services.GetRequiredService<IUserSessionService>();
                if (session.CurrentUser != null)
                {
                    MessageBox.Show("Su sesión ha expirado por inactividad.", "Seguridad", MessageBoxButton.OK, MessageBoxImage.Warning);
                    
                    // Disparar Logout
                    var mainVm = System.Windows.Application.Current.Windows.OfType<MainView>().FirstOrDefault()?.DataContext as MainViewModel;
                    if (mainVm != null)
                    {
                        mainVm.LogoutCommand.Execute(null);
                    }
                    else 
                    {
                        // Fallback si no hay MainView
                        session.CurrentUser = null;
                        var secureConfig = _host.Services.GetRequiredService<ISecureConfigService>();
                        secureConfig.ClearSession();
                        WeakReferenceMessenger.Default.Send(new LogoutMessage());
                    }
                }
            });
        };
    }

    private void LogError(string source, Exception? ex)
    {
        var message = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{source}] ERROR: {ex?.Message}\nDetalles: {ex?.StackTrace}\nInner: {ex?.InnerException?.Message}\n\n";
        File.AppendAllText(LogPath, message);
        MessageBox.Show($"Ocurrió un error inesperado. Consulte el log para más detalles:\n\n{ex?.Message}", "Error Crítico", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        LogError("UI Thread", e.Exception);
        e.Handled = true;
    }

    private void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        LogError("AppDomain", e.ExceptionObject as Exception);
    }

    private void OnTaskSchedulerUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        LogError("TaskScheduler", e.Exception);
        e.SetObserved();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        try 
        {
            _host.Start();

            using (var scope = _host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<OrionDbContext>();
                var secureConfig = scope.ServiceProvider.GetRequiredService<ISecureConfigService>();
                var usuarioService = scope.ServiceProvider.GetRequiredService<IUsuarioService>();
                var sessionService = scope.ServiceProvider.GetRequiredService<IUserSessionService>();
                
                var userConfig = secureConfig.LoadConfig();
                
                // 1. Inicializar DB
                DbInitializer.Initialize(context, userConfig.IsProduction);
                if (!userConfig.IsProduction) StagingSeedData.Seed(context);

                // 2. Intentar Auto-Login
                if (userConfig.RememberMe && userConfig.LastUserId.HasValue && userConfig.SessionExpiry > DateTime.UtcNow)
                {
                    var user = await usuarioService.GetByIdAsync(userConfig.LastUserId.Value);
                    if (user != null && user.Activo)
                    {
                        sessionService.CurrentUser = user;
                        var mainView = _host.Services.GetRequiredService<MainView>();
                        mainView.Show();
                        base.OnStartup(e);
                        return;
                    }
                }
            }

            _loginView = _host.Services.GetRequiredService<LoginView>();
            _loginView.Show();

            base.OnStartup(e);
        }
        catch (Exception ex)
        {
            LogError("Startup", ex);
            Shutdown();
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync();
        }
        base.OnExit(e);
    }
}
