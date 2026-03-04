using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;

namespace Orión.DesktopUI.ViewModels;

public partial class MaquinariaViewModel : ObservableObject
{
    private readonly IMaquinariaService _maquinariaService;

    [ObservableProperty]
    private ObservableCollection<MaquinariaDto> _maquinas = new();

    [ObservableProperty]
    private bool _isBusy;

    public MaquinariaViewModel(IMaquinariaService maquinariaService)
    {
        _maquinariaService = maquinariaService;
    }

    [RelayCommand]
    public async Task LoadMaquinasAsync()
    {
        IsBusy = true;
        try
        {
            var data = await _maquinariaService.GetAllDtoAsync();
            Maquinas = new ObservableCollection<MaquinariaDto>(data);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
