using FluentAssertions;
using Moq;
using Orión.Application.DTOs;
using Orión.Application.Interfaces;
using Orión.DesktopUI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Orión.Tests.ViewModels;

public class MaquinariaViewModelTests
{
    private readonly Mock<IMaquinariaService> _maquinariaServiceMock;
    private readonly Mock<IUserSessionService> _sessionServiceMock;
    private readonly Mock<IOrionDbContext> _contextMock;

    public MaquinariaViewModelTests()
    {
        _maquinariaServiceMock = new Mock<IMaquinariaService>();
        _sessionServiceMock = new Mock<IUserSessionService>();
        _contextMock = new Mock<IOrionDbContext>();
    }

    [Fact]
    public async Task SearchText_Should_Filter_Maquinaria_By_Nombre()
    {
        // Arrange
        var maquinas = new List<MaquinariaDto>
        {
            new MaquinariaDto { IdMaquinaria = "MAQ-01", NombreMaquina = "Prensa" },
            new MaquinariaDto { IdMaquinaria = "MAQ-02", NombreMaquina = "Torno" }
        };

        _maquinariaServiceMock.Setup(s => s.GetAllDtoAsync(It.IsAny<bool>())).ReturnsAsync(maquinas);
        var vm = new MaquinariaViewModel(_maquinariaServiceMock.Object, _sessionServiceMock.Object, _contextMock.Object);
        await vm.LoadMaquinariasAsync();

        // Act
        vm.SearchText = "Prensa";

        // Assert
        vm.Maquinarias.Should().HaveCount(1);
        vm.Maquinarias.First().NombreMaquina.Should().Be("Prensa");
    }

    [Fact]
    public async Task SearchText_Should_Filter_Maquinaria_By_Id()
    {
        // Arrange
        var maquinas = new List<MaquinariaDto>
        {
            new MaquinariaDto { IdMaquinaria = "MAQ-01", NombreMaquina = "Prensa" },
            new MaquinariaDto { IdMaquinaria = "MAQ-02", NombreMaquina = "Torno" }
        };

        _maquinariaServiceMock.Setup(s => s.GetAllDtoAsync(It.IsAny<bool>())).ReturnsAsync(maquinas);
        var vm = new MaquinariaViewModel(_maquinariaServiceMock.Object, _sessionServiceMock.Object, _contextMock.Object);
        await vm.LoadMaquinariasAsync();

        // Act
        vm.SearchText = "MAQ-02";

        // Assert
        vm.Maquinarias.Should().HaveCount(1);
        vm.Maquinarias.First().IdMaquinaria.Should().Be("MAQ-02");
    }
}
