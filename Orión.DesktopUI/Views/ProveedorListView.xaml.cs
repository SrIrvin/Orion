using Orión.DesktopUI.ViewModels;
using System.Windows.Controls;

namespace Orión.DesktopUI.Views;

public partial class ProveedorListView : UserControl
{
    public ProveedorListView(ProveedorViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
