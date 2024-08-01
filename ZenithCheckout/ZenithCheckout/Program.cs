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
services.AddScoped<ICheckout, Checkout>();

var serviceProvider = services.BuildServiceProvider();


//Program implementation
var checkout = serviceProvider.GetService<ICheckout>();

Console.WriteLine("Hello, World!");
