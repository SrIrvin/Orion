using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orión.Application.Interfaces;
using Orión.Application.Services;
using Orión.DesktopUI.ViewModels;
using Orión.DesktopUI.Views;
using Orión.Infrastructure.Persistence;

namespace Orión.DesktopUI;

public partial class App : System.Windows.Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Persistencia
                services.AddDbContext<OrionDbContext>(options =>
                    options.UseNpgsql("Host=localhost;Port=5433;Database=DB_Orion;Username=admin;Password=Mast3rC0mput0;CommandTimeout=30;Trust Server Certificate=true"));
                
                services.AddScoped<IOrionDbContext>(provider => provider.GetRequiredService<OrionDbContext>());

                // Servicios de Aplicación
                services.AddScoped<IAuthService, AuthService>();

                // ViewModels
                services.AddTransient<LoginViewModel>();

                // Views
                services.AddTransient<LoginView>();
            })
            .Build();
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

            var loginView = _host.Services.GetRequiredService<LoginView>();
            loginView.Show();

            base.OnStartup(e);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error crítico al iniciar la aplicación:\n\n{ex.Message}\n\nDetalles: {ex.InnerException?.Message}", 
                            "Error de Inicio", MessageBoxButton.OK, MessageBoxImage.Error);
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
