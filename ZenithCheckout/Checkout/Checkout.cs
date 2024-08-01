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
            totalPrice += CalculateTotal(item.Key, item.Value);
        }

        return totalPrice; 
    }

    private int CalculateTotal(char item, int amount)
    {
        int total = 0;

        if (_specialPrices.ContainsKey(item))
        {
            int sets = amount / _specialPrices[item]._setAmount;
            int specialTotal = sets * _specialPrices[item]._setPrice;
            int remainder = amount % _specialPrices[item]._setAmount;

            total += specialTotal + _itemPrices[item] * remainder;
        }
        else
        {
            total += _itemPrices[item] * amount;
        }

        return total;
    }

    //Test Helper Functions
    internal Dictionary<char, int> GetScannedItems() { return _scannedItems; }
    internal Dictionary<char, int> GetItemPrices() { return _itemPrices; }
    internal Dictionary<char, SpecialPrice> GetSpecialPrices() { return _specialPrices; }
}
