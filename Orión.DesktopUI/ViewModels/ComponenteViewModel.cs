using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;

namespace Orión.DesktopUI.ViewModels;

public partial class ComponenteViewModel : ObservableObject
{
    private readonly IComponenteService _componenteService;

    [ObservableProperty]
    private ObservableCollection<ComponenteDto> _componentes = new();

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _maquinariaId = string.Empty;

    public ComponenteViewModel(IComponenteService componenteService)
    {
        _componenteService = componenteService;
    }

    [RelayCommand]
    public async Task LoadComponentesAsync()
    {
        if (string.IsNullOrEmpty(MaquinariaId)) return;

        IsBusy = true;
        try
        {
            var data = await _componenteService.GetByMaquinariaIdDtoAsync(MaquinariaId);
            Componentes = new ObservableCollection<ComponenteDto>(data);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
