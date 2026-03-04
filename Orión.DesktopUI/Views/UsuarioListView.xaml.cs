using System.Windows.Controls;
using Orión.DesktopUI.ViewModels;

namespace Orión.DesktopUI.Views;

public partial class UsuarioListView : UserControl
{
    public UsuarioListView(UsuarioViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        
        Loaded += async (s, e) => await viewModel.LoadUsuariosAsync();
    }
}
