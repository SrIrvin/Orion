using FluentAssertions;
using Moq;
using Orión.Application.Interfaces;
using Orión.DesktopUI.Interfaces;
using Orión.DesktopUI.ViewModels;
using Orión.DesktopUI.Views;
using Xunit;

namespace Orión.Tests.ViewModels;

public class MainViewModelTests
{
    [Fact]
    public void Constructor_Should_Navigate_To_Dashboard()
    {
        // Arrange
        var mockNav = new Mock<INavigationService>();
        var mockSession = new Mock<IUserSessionService>();

        // Act
        var vm = new MainViewModel(mockNav.Object, mockSession.Object);

        // Assert
        mockNav.Verify(n => n.NavigateTo<DashboardView>(), Times.Once);
    }

    [Fact]
    public void NavigateToMaquinaria_Should_Call_NavigationService()
    {
        // Arrange
        var mockNav = new Mock<INavigationService>();
        var mockSession = new Mock<IUserSessionService>();
        var vm = new MainViewModel(mockNav.Object, mockSession.Object);

        // Act
        vm.NavigateToMaquinariaCommand.Execute(null);

        // Assert
        mockNav.Verify(n => n.NavigateTo<MaquinariaListView>(), Times.Once);
    }

    [Fact]
    public void NavigateToUsuarios_Should_Only_Work_For_Admin()
    {
        // Arrange
        var mockNav = new Mock<INavigationService>();
        var mockSession = new Mock<IUserSessionService>();
        mockSession.Setup(s => s.IsAdmin).Returns(false);
        
        var vm = new MainViewModel(mockNav.Object, mockSession.Object);

        // Act
        vm.NavigateToUsuariosCommand.Execute(null);

        // Assert
        mockNav.Verify(n => n.NavigateTo<UsuarioListView>(), Times.Never);
    }
}
