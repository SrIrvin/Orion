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

public class TecnicoViewModelTests
{
    private readonly Mock<ITecnicoService> _tecnicoServiceMock;
    private readonly Mock<IUserSessionService> _sessionServiceMock;
    private readonly Mock<IOrionDbContext> _contextMock;

    public TecnicoViewModelTests()
    {
        _tecnicoServiceMock = new Mock<ITecnicoService>();
        _sessionServiceMock = new Mock<IUserSessionService>();
        _contextMock = new Mock<IOrionDbContext>();
    }

    [Fact]
    public async Task SearchText_Should_Filter_Tecnicos_By_Nombre()
    {
        // Arrange
        var tecnicos = new List<TecnicoDto>
        {
            new TecnicoDto { IdPersonal = 1, NombreApellido = "Juan Perez", Especialidad = "Mecanico" },
            new TecnicoDto { IdPersonal = 2, NombreApellido = "Maria Garcia", Especialidad = "Electrico" }
        };

        _tecnicoServiceMock.Setup(s => s.GetAllDtoAsync(It.IsAny<bool>())).ReturnsAsync(tecnicos);
        _sessionServiceMock.Setup(s => s.IsAdmin).Returns(true);

        var vm = new TecnicoViewModel(_tecnicoServiceMock.Object, _sessionServiceMock.Object, _contextMock.Object);
        await vm.LoadTecnicosAsync();

        // Act
        vm.SearchText = "Juan";

        // Assert
        vm.Tecnicos.Should().HaveCount(1);
        vm.Tecnicos.First().NombreApellido.Should().Be("Juan Perez");
    }

    [Fact]
    public async Task SearchText_Should_Filter_Tecnicos_By_Especialidad()
    {
        // Arrange
        var tecnicos = new List<TecnicoDto>
        {
            new TecnicoDto { IdPersonal = 1, NombreApellido = "Juan Perez", Especialidad = "Mecanico" },
            new TecnicoDto { IdPersonal = 2, NombreApellido = "Maria Garcia", Especialidad = "Electrico" }
        };

        _tecnicoServiceMock.Setup(s => s.GetAllDtoAsync(It.IsAny<bool>())).ReturnsAsync(tecnicos);
        _sessionServiceMock.Setup(s => s.IsAdmin).Returns(true);

        var vm = new TecnicoViewModel(_tecnicoServiceMock.Object, _sessionServiceMock.Object, _contextMock.Object);
        await vm.LoadTecnicosAsync();

        // Act
        vm.SearchText = "Electrico";

        // Assert
        vm.Tecnicos.Should().HaveCount(1);
        vm.Tecnicos.First().NombreApellido.Should().Be("Maria Garcia");
    }

    [Fact]
    public async Task SearchText_Empty_Should_Show_All_Tecnicos()
    {
        // Arrange
        var tecnicos = new List<TecnicoDto>
        {
            new TecnicoDto { IdPersonal = 1, NombreApellido = "Juan Perez" },
            new TecnicoDto { IdPersonal = 2, NombreApellido = "Maria Garcia" }
        };

        _tecnicoServiceMock.Setup(s => s.GetAllDtoAsync(It.IsAny<bool>())).ReturnsAsync(tecnicos);
        var vm = new TecnicoViewModel(_tecnicoServiceMock.Object, _sessionServiceMock.Object, _contextMock.Object);
        await vm.LoadTecnicosAsync();

        // Act
        vm.SearchText = "Juan";
        vm.SearchText = "";

        // Assert
        vm.Tecnicos.Should().HaveCount(2);
    }
}
