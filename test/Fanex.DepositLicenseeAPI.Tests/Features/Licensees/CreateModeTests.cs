namespace Fanex.DepositLicenseeAPI.Tests.Features.Licensees;

using DepositLicenseeAPI.Features.Licensees;

public class CreateModeTests
{
    [Fact]
    public void NewLicensee_HasCorrectValue()
    {
        // Act & Assert
        CreateMode.NewLicensee.Should().Be(1);
    }

    [Fact]
    public void NewCurrency_HasCorrectValue()
    {
        // Act & Assert
        CreateMode.NewCurrency.Should().Be(2);
    }

    [Fact]
    public void Constants_AreOfTypeByteAndDistinct()
    {
        // Act & Assert
        CreateMode.NewLicensee.Should().BeOfType(typeof(byte));
        CreateMode.NewCurrency.Should().BeOfType(typeof(byte));
        CreateMode.NewLicensee.Should().NotBe(CreateMode.NewCurrency);
    }
}