using System.Windows;
using Orión.DesktopUI.ViewModels;

namespace Orión.DesktopUI.Views;

public partial class MainView : Window
{
    public MainView(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
