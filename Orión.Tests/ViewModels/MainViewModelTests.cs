using FluentAssertions;
using Moq;
using Microsoft.Extensions.Configuration;
using Orión.Application.Interfaces;
using Orión.Application.DTOs;
using Orión.DesktopUI.Interfaces;
using Orión.DesktopUI.ViewModels;
using Orión.DesktopUI.Views;
using Xunit;

namespace Orión.Tests.ViewModels;

public class MainViewModelTests
{
    private readonly Mock<INavigationService> _mockNav;
    private readonly Mock<IUserSessionService> _mockSession;
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly Mock<ISecureConfigService> _mockSecureConfig;

    public MainViewModelTests()
    {
        _mockNav = new Mock<INavigationService>();
        _mockSession = new Mock<IUserSessionService>();
        _mockConfig = new Mock<IConfiguration>();
        _mockSecureConfig = new Mock<ISecureConfigService>();

        _mockSecureConfig.Setup(s => s.LoadConfig()).Returns(new DbConfigurationDto());
    }

    [Fact]
    public void Constructor_Should_Navigate_To_Dashboard()
    {
        // Act
        var vm = new MainViewModel(_mockNav.Object, _mockSession.Object, _mockConfig.Object, _mockSecureConfig.Object);

        // Assert
        _mockNav.Verify(n => n.NavigateTo<DashboardView>(), Times.Once);
    }

    [Fact]
    public void NavigateToMaquinaria_Should_Call_NavigationService()
    {
        // Arrange
        var vm = new MainViewModel(_mockNav.Object, _mockSession.Object, _mockConfig.Object, _mockSecureConfig.Object);

        // Act
        vm.NavigateToMaquinariaCommand.Execute(null);

        // Assert
        _mockNav.Verify(n => n.NavigateTo<MaquinariaListView>(), Times.Once);
    }

    [Fact]
    public void NavigateToUsuarios_Should_Only_Work_For_Admin()
    {
        // Arrange
        _mockSession.Setup(s => s.IsAdmin).Returns(false);
        var vm = new MainViewModel(_mockNav.Object, _mockSession.Object, _mockConfig.Object, _mockSecureConfig.Object);

        // Act
        vm.NavigateToUsuariosCommand.Execute(null);

        // Assert
        _mockNav.Verify(n => n.NavigateTo<UsuarioListView>(), Times.Never);
    }
}
