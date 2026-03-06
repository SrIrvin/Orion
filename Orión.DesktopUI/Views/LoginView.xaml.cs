using System.Windows;
using Orión.DesktopUI.ViewModels;

namespace Orión.DesktopUI.Views;

public partial class LoginView : Window
{
    public LoginView(LoginViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        // Ya no es necesario pasar la contraseña manualmente, 
        // el Binding bidireccional a través de PasswordBoxHelper lo maneja automáticamente.
    }
}
