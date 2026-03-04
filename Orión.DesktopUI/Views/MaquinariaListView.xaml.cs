using System.Windows.Controls;
using Orión.DesktopUI.ViewModels;

namespace Orión.DesktopUI.Views;

public partial class MaquinariaListView : UserControl
{
    public MaquinariaListView(MaquinariaViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        
        // Cargar datos al iniciar
        Loaded += async (s, e) => await viewModel.LoadMaquinasAsync();
    }
}
