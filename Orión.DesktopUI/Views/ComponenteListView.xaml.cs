using System.Windows.Controls;
using Orión.DesktopUI.ViewModels;

namespace Orión.DesktopUI.Views;

public partial class ComponenteListView : UserControl
{
    public ComponenteListView(ComponenteViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        Loaded += async (s, e) => await viewModel.LoadComponentesAsync();
    }
}
