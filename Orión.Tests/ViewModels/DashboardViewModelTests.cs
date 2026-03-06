using FluentAssertions;
using Moq;
using Orión.Application.Interfaces;
using Orión.DesktopUI.ViewModels;
using Orión.Domain.Entities;
using Xunit;

namespace Orión.Tests.ViewModels;

public class DashboardViewModelTests
{
    private readonly Mock<IDashboardService> _mockDashboard;
    private readonly Mock<IUserSessionService> _mockSession;

    public DashboardViewModelTests()
    {
        _mockDashboard = new Mock<IDashboardService>();
        _mockSession = new Mock<IUserSessionService>();
    }

    [Fact]
    public void Constructor_Should_ShowSecurityWarning_When_UsingDefaultAdmin()
    {
        // Arrange
        var defaultAdmin = new Usuario
        {
            NombreUsuario = "admin",
            RequiresPasswordChange = true
        };
        _mockSession.Setup(s => s.CurrentUser).Returns(defaultAdmin);

        // Act
        var vm = new DashboardViewModel(_mockDashboard.Object, _mockSession.Object);

        // Assert
        vm.ShowSecurityWarning.Should().BeTrue();
    }

    [Fact]
    public void DismissSecurityWarning_Should_HideBanner()
    {
        // Arrange
        var vm = new DashboardViewModel(_mockDashboard.Object, _mockSession.Object);
        vm.ShowSecurityWarning = true;

        // Act
        vm.DismissSecurityWarningCommand.Execute(null);

        // Assert
        vm.ShowSecurityWarning.Should().BeFalse();
    }
}
