using FluentAssertions;
using Orión.Domain.Entities;
using Xunit;

namespace Orión.Tests.Domain;

public class MaquinariaTests
{
    [Fact]
    public void Maquinaria_Should_Have_Empty_Components_List_On_Creation()
    {
        // Arrange & Act
        var maquinaria = new Maquinaria();

        // Assert
        maquinaria.Componentes.Should().NotBeNull();
        maquinaria.Componentes.Should().BeEmpty();
    }

    [Fact]
    public void Maquinaria_Should_Allow_Adding_Components()
    {
        // Arrange
        var maquinaria = new Maquinaria { IdMaquinaria = "MAQ01", NombreMaquina = "Prensa" };
        var componente = new Componente { IdComponente = "COMP01", NombreComponente = "Motor" };

        // Act
        maquinaria.Componentes.Add(componente);

        // Assert
        maquinaria.Componentes.Should().HaveCount(1);
        maquinaria.Componentes.Should().Contain(componente);
    }
}
