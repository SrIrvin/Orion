using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Orión.Application.Interfaces;
using System.Threading.Tasks;

namespace Orión.DesktopUI.ViewModels;

public partial class ReportViewModel : ObservableObject
{
    private readonly IMaquinariaService _maquinariaService;
    private readonly ITecnicoService _tecnicoService;

    [ObservableProperty]
    private bool _isBusy;

    public ReportViewModel(IMaquinariaService maquinariaService, ITecnicoService tecnicoService)
    {
        _maquinariaService = maquinariaService;
        _tecnicoService = tecnicoService;
    }

    [RelayCommand]
    private async Task ExportMaquinariaReport()
    {
        IsBusy = true;
        // Simulación de exportación o lógica de reporte
        await Task.Delay(1000); 
        IsBusy = false;
        
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "PDF files (*.pdf)|*.pdf",
            FileName = "Reporte_Maquinaria.pdf"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            // Aquí iría la lógica real de generación de PDF masivo si el servicio lo soportara
        }
    }
}
