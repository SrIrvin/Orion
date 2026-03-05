using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;
using Orión.Domain.Entities;
using System.Collections.ObjectModel;

namespace Orión.DesktopUI.ViewModels;

public partial class ProveedorViewModel : ObservableObject
{
    private readonly IProveedorService _proveedorService;

    [ObservableProperty]
    private ObservableCollection<ProveedorDto> _proveedores = new();

    [ObservableProperty]
    private ProveedorDto? _selectedProveedor;

    public ProveedorViewModel(IProveedorService proveedorService)
    {
        _proveedorService = proveedorService;
        LoadDataCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadData()
    {
        var data = await _proveedorService.GetAllDtoAsync(includeInactive: true);
        Proveedores = new ObservableCollection<ProveedorDto>(data);
    }

    [RelayCommand]
    private async Task ToggleStatus(ProveedorDto? dto)
    {
        if (dto == null) return;
        await _proveedorService.ToggleStatusAsync(dto.IdProveedor);
        await LoadData();
    }
}
