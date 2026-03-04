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
using Orión.Infrastructure.Persistence;
using Orión.Infrastructure.Repositories;

namespace Orión.DesktopUI;

public partial class App : System.Windows.Application
{
    private readonly IHost _host;
    private LoginView? _loginView;
    private static readonly string LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");

    public App()
    {
        // 1. Configurar Handlers de Excepciones Globales
        DispatcherUnhandledException += OnDispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
        TaskScheduler.UnobservedTaskException += OnTaskSchedulerUnobservedTaskException;

        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Persistencia
                services.AddDbContext<OrionDbContext>(options =>
                    options.UseNpgsql("Host=localhost;Port=5433;Database=DB_Orion;Username=admin;Password=Mast3rC0mput0;CommandTimeout=30;Trust Server Certificate=true"));
                
                services.AddScoped<IOrionDbContext>(provider => provider.GetRequiredService<OrionDbContext>());

                // Repositorios
                services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

                // Servicios de Aplicación
                services.AddScoped<IAuthService, AuthService>();
                services.AddScoped<IMaquinariaService, MaquinariaService>();
                services.AddScoped<IComponenteService, ComponenteService>();
                services.AddScoped<ITecnicoService, TecnicoService>();

                // ViewModels
                services.AddTransient<LoginViewModel>();
                services.AddTransient<MainViewModel>();
                services.AddTransient<MaquinariaViewModel>();
                services.AddTransient<ComponenteViewModel>();
                services.AddTransient<TecnicoViewModel>();

                // Views
                services.AddTransient<LoginView>();
                services.AddTransient<MainView>();
                services.AddTransient<DashboardView>();
                services.AddTransient<MaquinariaListView>();
                services.AddTransient<ComponenteListView>();
                services.AddTransient<TecnicoListView>();
            })
            .Build();

        // Suscribirse al mensaje de login
        WeakReferenceMessenger.Default.Register<LoginSuccessMessage>(this, (r, m) =>
        {
            var mainView = _host.Services.GetRequiredService<MainView>();
            mainView.Show();
            _loginView?.Close();
        });
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

    protected override void OnStartup(StartupEventArgs e)
    {
        try 
        {
            _host.Start();

            // Asegurar que la base de datos esté creada y migrada
            using (var scope = _host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<OrionDbContext>();
                DbInitializer.Initialize(context);
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
