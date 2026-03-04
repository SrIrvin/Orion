using System.Windows;
using System.Windows.Controls;
using Orión.DesktopUI.ViewModels;

namespace Orión.DesktopUI.Views;

public partial class UsuarioEditorView : UserControl
{
    public UsuarioEditorView()
    {
        InitializeComponent();
    }

    private async void Save_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is UsuarioEditorViewModel vm)
        {
            // Pasar la contraseña solo si es un usuario nuevo
            if (vm.IsNew)
            {
                vm.Password = PasswordBox.Password;
            }
            
            await vm.SaveCommand.ExecuteAsync(null);
        }
    }
}
