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
    private readonly Mock<IMessageService> _mockMessage;

    public MainViewModelTests()
    {
        _mockNav = new Mock<INavigationService>();
        _mockSession = new Mock<IUserSessionService>();
        _mockConfig = new Mock<IConfiguration>();
        _mockSecureConfig = new Mock<ISecureConfigService>();
        _mockMessage = new Mock<IMessageService>();

        _mockSecureConfig.Setup(s => s.LoadConfig()).Returns(new DbConfigurationDto());
    }

    [Fact]
    public void Constructor_Should_Navigate_To_Dashboard()
    {
        // Act
        var vm = new MainViewModel(_mockNav.Object, _mockSession.Object, _mockConfig.Object, _mockSecureConfig.Object, _mockMessage.Object);

        // Assert
        _mockNav.Verify(n => n.NavigateTo<DashboardView>(), Times.Once);
    }

    [Fact]
    public void NavigateToMaquinaria_Should_Call_NavigationService()
    {
        // Arrange
        var vm = new MainViewModel(_mockNav.Object, _mockSession.Object, _mockConfig.Object, _mockSecureConfig.Object, _mockMessage.Object);

        // Act
        vm.NavigateToMaquinariaCommand.Execute(null);

        // Assert
        _mockNav.Verify(n => n.NavigateTo<MaquinariaListView>(), Times.Once);
    }

    [Fact]
    public async Task TestConnectionCommand_ShouldUpdateStatusProperties()
    {
        // Arrange
        _mockSecureConfig.Setup(s => s.TestConnection(It.IsAny<DbConfigurationDto>()))
                         .ReturnsAsync(true);
        var vm = new MainViewModel(_mockNav.Object, _mockSession.Object, _mockConfig.Object, _mockSecureConfig.Object, _mockMessage.Object);

        // Act
        await vm.TestConnectionCommand.ExecuteAsync(null);

        // Assert
        vm.IsTestSuccessful.Should().BeTrue();
        vm.TestResultMessage.Should().Contain("exitosa");
    }

    [Fact]
    public void SaveConfigurationCommand_ShouldInvokeSecureConfigService()
    {
        // Arrange
        var vm = new MainViewModel(_mockNav.Object, _mockSession.Object, _mockConfig.Object, _mockSecureConfig.Object, _mockMessage.Object);
        var testConfig = new DbConfigurationDto { Provider = "Access", AccessFilePath = "test.accdb" };
        vm.ConfigEditBuffer = testConfig;

        // Act
        try { vm.SaveConfigurationCommand.Execute(null); } catch { /* Ignore UI closing error in tests (DialogHost) */ }

        // Assert
        _mockSecureConfig.Verify(s => s.SaveConfig(It.Is<DbConfigurationDto>(c => c.Provider == "Access")), Times.Once);
        _mockMessage.Verify(m => m.ShowInfo(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
}
