// See https://aka.ms/new-console-template for more information

using CheckoutLibrary;
using CheckoutLibrary.Interfaces;
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

services.AddSingleton(_unitPrices);
services.AddSingleton(_specialPrices);
services.AddSingleton<ICheckout, Checkout>();

var serviceProvider = services.BuildServiceProvider();


//Program implementation
var checkout = serviceProvider.GetService<ICheckout>();

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

    Console.WriteLine($"Scanning item: {itemToScan}");
    checkout.Scan(itemToScan[0]);
}

Console.WriteLine("Calculating total");

var totalPrice = checkout.GetTotalPrice();

Console.WriteLine($"Total Price: {totalPrice}");

Console.WriteLine("Press any key to close...");
Console.ReadKey(true);