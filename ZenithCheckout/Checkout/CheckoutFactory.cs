using CheckoutLibrary.Interfaces;
using CheckoutLibrary.Models;
using CheckoutLibrary;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CheckoutFactoryTests")]
namespace CheckoutLibrary;

public class CheckoutFactory
{
    private static readonly Dictionary<string, ICheckout> _checkouts = new Dictionary<string, ICheckout>();

    public static void RegisterCheckouts(IServiceCollection services, 
        params (string name, Dictionary<char, int> itemPrices, Dictionary<char, SpecialPrice> specialPrices)[] checkouts)
    {
        foreach (var (name, itemPrices, specialPrices) in checkouts)
        {
            _checkouts.Add(name, new Checkout(itemPrices, specialPrices));
        }

        services.AddSingleton(_checkouts);
    }

    public static ICheckout GetService(IServiceProvider serviceProvider, string name)
    {
        var checkouts = serviceProvider.GetService<Dictionary<string, ICheckout>>();
        return checkouts[name];
    }

    //Test helper methods
    internal static void ResetCheckoutDictionary() { _checkouts.Clear(); }
}