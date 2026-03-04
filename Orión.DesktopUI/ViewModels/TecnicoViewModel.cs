using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;

namespace Orión.DesktopUI.ViewModels;

public partial class TecnicoViewModel : ObservableObject
{
    private readonly ITecnicoService _tecnicoService;

    [ObservableProperty]
    private ObservableCollection<TecnicoDto> _tecnicos = new();

    [ObservableProperty]
    private bool _isBusy;

    public TecnicoViewModel(ITecnicoService tecnicoService)
    {
        _tecnicoService = tecnicoService;
    }

    [RelayCommand]
    public async Task LoadTecnicosAsync()
    {
        IsBusy = true;
        try
        {
            var data = await _tecnicoService.GetAllDtoAsync();
            Tecnicos = new ObservableCollection<TecnicoDto>(data);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
