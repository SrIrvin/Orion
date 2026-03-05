using Orión.DesktopUI.ViewModels;
using System.Windows.Controls;

namespace Orión.DesktopUI.Views;

public partial class ReportView : UserControl
{
    public ReportView(ReportViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
