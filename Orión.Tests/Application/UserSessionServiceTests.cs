using FluentAssertions;
using Orión.Application.Services;
using Orión.Domain.Entities;
using Xunit;

namespace Orión.Tests.Application;

public class UserSessionServiceTests
{
    [Fact]
    public void IsAdmin_Should_Be_True_When_User_Has_Admin_Rol()
    {
        // Arrange
        var service = new UserSessionService();
        service.CurrentUser = new Usuario { NombreUsuario = "admin", Rol = "Admin" };

        // Act & Assert
        service.IsAdmin.Should().BeTrue();
    }

    [Fact]
    public void IsAdmin_Should_Be_False_When_User_Has_Operator_Rol()
    {
        // Arrange
        var service = new UserSessionService();
        service.CurrentUser = new Usuario { NombreUsuario = "operador", Rol = "Operador" };

        // Act & Assert
        service.IsAdmin.Should().BeFalse();
    }

    [Fact]
    public void IsAdmin_Should_Be_False_When_No_User_Logged_In()
    {
        // Arrange
        var service = new UserSessionService();

        // Act & Assert
        service.IsAdmin.Should().BeFalse();
    }
}
