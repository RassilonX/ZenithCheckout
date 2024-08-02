using CheckoutLibrary;
using CheckoutLibrary.Interfaces;
using CheckoutLibrary.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CheckoutTests;

public class CheckoutFactoryTests
{
    [Fact]
    public void RegisterCheckouts_RegistersCheckoutsCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        var checkouts = new[]
        {
            ("default", new Dictionary<char, int> { { 'A', 10 } }, new Dictionary<char, SpecialPrice> { { 'A', new SpecialPrice(3, 20) } }),
            ("premium", new Dictionary<char, int> { { 'B', 20 } }, new Dictionary<char, SpecialPrice> { { 'B', new SpecialPrice(2, 30) } }),
        };

        // Act
        CheckoutFactory.RegisterCheckouts(services, checkouts);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var registeredCheckouts = serviceProvider.GetService<Dictionary<string, ICheckout>>();
        Assert.NotNull(registeredCheckouts);
        Assert.True(registeredCheckouts.ContainsKey("default"));
        Assert.True(registeredCheckouts.ContainsKey("premium"));

        CheckoutFactory.ResetCheckoutDictionary();
    }

    [Fact]
    public void GetService_ReturnsCorrectCheckoutInstance()
    {
        // Arrange
        var services = new ServiceCollection();
        var checkouts = new[]
        {
            ("default", new Dictionary<char, int> { { 'A', 10 } }, new Dictionary<char, SpecialPrice> { { 'A', new SpecialPrice(3, 20) } }),
        };
        CheckoutFactory.RegisterCheckouts(services, checkouts);

        // Act
        var serviceProvider = services.BuildServiceProvider();
        var defaultCheckout = CheckoutFactory.GetService(serviceProvider, "default");

        // Assert
        Assert.NotNull(defaultCheckout);
        Assert.IsAssignableFrom<Checkout>(defaultCheckout);

        CheckoutFactory.ResetCheckoutDictionary();
    }

    [Fact]
    public void GetService_ThrowsException_WhenCheckoutNameIsNotFound()
    {
        // Arrange
        var services = new ServiceCollection();
        var checkouts = new[]
        {
            ("default", new Dictionary<char, int> { { 'A', 10 } }, new Dictionary<char, SpecialPrice> { { 'A', new SpecialPrice(3, 20) } }),
        };
        CheckoutFactory.RegisterCheckouts(services, checkouts);

        // Act and Assert
        var serviceProvider = services.BuildServiceProvider();
        Assert.Throws<KeyNotFoundException>(() => CheckoutFactory.GetService(serviceProvider, "non-existent"));

        CheckoutFactory.ResetCheckoutDictionary();
    }
}
