using CheckoutLibrary.Interfaces;
using CheckoutLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("CheckoutTests")]
namespace CheckoutLibrary;

public class Checkout : ICheckout
{
    private Dictionary<char, int> _scannedItems = new Dictionary<char, int>();
    private readonly Dictionary<char, int> _itemPrices;
    private readonly Dictionary<char, SpecialPrice> _specialPrices;

    public Checkout(Dictionary<char, int> itemPrices, Dictionary<char, SpecialPrice> specialPrices)
    {
        _itemPrices = itemPrices ?? new Dictionary<char, int>();
        _specialPrices = specialPrices ?? new Dictionary<char, SpecialPrice>();
    }

    public void Scan(char item)
    {
        if (_scannedItems.ContainsKey(item))
        {
            _scannedItems[item]++;
        }
        else 
        {
            _scannedItems.Add(item, 1);
        }
    }

    public int GetTotalPrice() 
    {
        int totalPrice = 0;

        foreach (var item in _scannedItems)
        {
            totalPrice += _itemPrices[item.Key] * item.Value;
        }

        return totalPrice; 
    }

    //Test Helper Functions
    internal Dictionary<char, int> GetScannedItems() { return _scannedItems; }
    internal Dictionary<char, int> GetItemPrices() { return _itemPrices; }
    internal Dictionary<char, SpecialPrice> GetSpecialPrices() { return _specialPrices; }
}
