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

public class UsuarioViewModelTests
{
    private readonly Mock<IUsuarioService> _usuarioServiceMock;

    public UsuarioViewModelTests()
    {
        _usuarioServiceMock = new Mock<IUsuarioService>();
    }

    [Fact]
    public async Task SearchText_Should_Filter_Usuarios_By_NombreUsuario()
    {
        // Arrange
        var usuarios = new List<UsuarioDto>
        {
            new UsuarioDto { IdUsuario = 1, NombreUsuario = "admin", Rol = "Admin" },
            new UsuarioDto { IdUsuario = 2, NombreUsuario = "operador", Rol = "Operador" }
        };

        _usuarioServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(usuarios);
        var vm = new UsuarioViewModel(_usuarioServiceMock.Object);
        await vm.LoadUsuariosAsync();

        // Act
        vm.SearchText = "admin";

        // Assert
        vm.Usuarios.Should().HaveCount(1);
        vm.Usuarios.First().NombreUsuario.Should().Be("admin");
    }

    [Fact]
    public async Task SearchText_Should_Filter_Usuarios_By_Rol()
    {
        // Arrange
        var usuarios = new List<UsuarioDto>
        {
            new UsuarioDto { IdUsuario = 1, NombreUsuario = "admin", Rol = "Admin" },
            new UsuarioDto { IdUsuario = 2, NombreUsuario = "operador", Rol = "Operador" }
        };

        _usuarioServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(usuarios);
        var vm = new UsuarioViewModel(_usuarioServiceMock.Object);
        await vm.LoadUsuariosAsync();

        // Act
        vm.SearchText = "Operador";

        // Assert
        vm.Usuarios.Should().HaveCount(1);
        vm.Usuarios.First().Rol.Should().Be("Operador");
    }
}
