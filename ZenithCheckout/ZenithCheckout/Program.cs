// See https://aka.ms/new-console-template for more information

using CheckoutLibrary;
using CheckoutLibrary.Models;
using Microsoft.Extensions.DependencyInjection;


//Checkout and Service setup
var _unitPrices =
    new Dictionary<char, int>
    {
            {'A', 50},
            {'B', 30},
            {'C', 20},
            {'D', 15}
    };

var _specialPrices =
    new Dictionary<char, SpecialPrice>
    {
            {'A', new SpecialPrice(3, 130) },
            {'B', new SpecialPrice(2, 45) }
    };


var services = new ServiceCollection();

CheckoutFactory.RegisterCheckouts(services,
    ("default", _unitPrices, _specialPrices));
var serviceProvider = services.BuildServiceProvider();


//Program implementation
var checkout = CheckoutFactory.GetService(serviceProvider, "default");

bool scanningItems = true;

Console.WriteLine("Please begin scanning items");
Console.WriteLine("When you are done please press enter on a blank line");

while (scanningItems)
{
    string itemToScan = Console.ReadLine();

    if(string.IsNullOrEmpty(itemToScan))
    {
        break;
    }

    try
    {
        Console.WriteLine($"Scanning item: {itemToScan}");
        checkout.Scan(itemToScan[0]);
    }
    catch (Exception ex) 
    {
        Console.WriteLine($"{ex.Message}");
    }
}

Console.WriteLine("Calculating total");

var totalPrice = checkout.GetTotalPrice();

Console.WriteLine($"Total Price: {totalPrice}");

Console.WriteLine("Press any key to close...");
Console.ReadKey(true);