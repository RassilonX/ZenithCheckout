using CheckoutLibrary.Interfaces;
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

    public Checkout(Dictionary<char, int> itemPrices)
    {
        _itemPrices = itemPrices ?? new Dictionary<char, int>();
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
        return 0; 
    }

    //Test Helper Functions
    internal Dictionary<char, int> GetScannedItems() { return _scannedItems; }
    internal Dictionary<char, int> GetItemPrices() { return _itemPrices; }
}
