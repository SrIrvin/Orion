using System.Windows.Controls;
using Orión.DesktopUI.ViewModels;

namespace Orión.DesktopUI.Views;

public partial class SolicitudListView : UserControl
{
    public SolicitudListView(SolicitudViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        Loaded += async (s, e) => await viewModel.LoadDataAsync();
    }
}
