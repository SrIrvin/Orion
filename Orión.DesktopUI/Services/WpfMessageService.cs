using System.Windows;
using Orión.Application.Interfaces;

namespace Orión.DesktopUI.Services;

public class WpfMessageService : IMessageService
{
    public void ShowInfo(string message, string title = "Información") => 
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

    public void ShowWarning(string message, string title = "Advertencia") => 
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);

    public void ShowError(string message, string title = "Error") => 
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);

    public bool ShowConfirmation(string message, string title = "Confirmación") => 
        MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
}
