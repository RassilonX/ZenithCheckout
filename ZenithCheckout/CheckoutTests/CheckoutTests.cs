using CheckoutLibrary;
using CheckoutLibrary.Models;

namespace CheckoutLibraryTests;

public class CheckoutTests
{
    private readonly Dictionary<char, int> _unitPrices =
        new Dictionary<char, int>
        {
            {'A', 50},
            {'B', 30},
            {'C', 20},
            {'D', 15}
        };

    private readonly Dictionary<char, SpecialPrice> _specialPrices =
        new Dictionary<char, SpecialPrice>
        {
            {'A', new SpecialPrice(3, 130) },
            {'B', new SpecialPrice(2, 45) }
        };

    [Fact]
    public void Checkout_NullItemPriceInConstructor_SetsToEmptyDictionary()
    {
        //Arrange - Act
        var checkout = new Checkout(null, null);

        var result = checkout.GetItemPrices();

        Assert.NotNull(result);
        Assert.IsType<Dictionary<char, int>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Checkout_NullSpecialPriceInConstructor_SetsToEmptyDictionary()
    {
        //Arrange - Act
        var checkout = new Checkout(null, null);

        var result = checkout.GetSpecialPrices();

        Assert.NotNull(result);
        Assert.IsType<Dictionary<char, SpecialPrice>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Checkout_ItemPrice_SetsToProvidedDictionary()
    {
        //Arrange - Act
        var checkout = new Checkout(_unitPrices, _specialPrices);

        var result = checkout.GetItemPrices();

        Assert.NotNull(result);
        Assert.IsType<Dictionary<char, int>>(result);
        Assert.Equal(result, _unitPrices);
    }

    [Fact]
    public void Checkout_SpecialPrice_SetsToProvidedDictionary()
    {
        //Arrange - Act
        var checkout = new Checkout(_unitPrices, _specialPrices);

        var result = checkout.GetSpecialPrices();

        Assert.NotNull(result);
        Assert.IsType<Dictionary<char, SpecialPrice>>(result);
        Assert.Equal(result, _specialPrices);
    }

    [Fact]
    public void Scan_ScanSingleItem_SingleItemPresentInScannedList()
    {
        //Arrange
        var checkout = new Checkout(_unitPrices, _specialPrices);

        //Act
        checkout.Scan('A');

        var result = checkout.GetScannedItems();

        //Assert
        Assert.Single(result);
        Assert.Equal('A', result.First().Key);
        Assert.Equal(1, result.First().Value);
        Assert.IsType<Dictionary<char, int>>(result);
    }

    [Fact]
    public void Scan_ScanMultipleItems_MultipleItemsPresentInScannedList()
    {
        //Arrange
        var checkout = new Checkout(_unitPrices, _specialPrices);
        var listTotal = _unitPrices.Sum(x => x.Value);

        //Act
        checkout.Scan('A');
        checkout.Scan('B');
        checkout.Scan('C');
        checkout.Scan('D');

        var result = checkout.GetScannedItems();

        char[] keysToCheck = ['A', 'B', 'C', 'D'];

        var allKeysPresent = keysToCheck.All(key => result.ContainsKey(key));

        //Assert
        Assert.Equal(4, result.Count);
        Assert.IsType<Dictionary<char, int>>(result);
        Assert.True(allKeysPresent);
    }


    [Fact]
    public void Scan_ItemNotInPriceList_ThrowsArgumentException()
    {
        //Arrange
        var checkout = new Checkout(_unitPrices, _specialPrices);

        //Act - Assert
        Assert.Throws<ArgumentException>(() => checkout.Scan('E'));
    }

    public static IEnumerable<object[]> NoSpecialOffersData =>
    new[]
    {
        new object[] { new char[] { 'A' }, 50 },
        new object[] { new char[] { 'A', 'B', 'C', 'D' }, 115 },
        new object[] { new char[] { 'A', 'A', 'B', 'C', 'D' }, 165 },
        new object[] { new char[] { 'A', 'B', 'A', 'C', 'D' }, 165 },
        new object[] { new char[] { }, 0 },
        // Add more test data here as appropriate
    };

    [Theory]
    [MemberData(nameof(NoSpecialOffersData))]
    public void GetTotalPrice_SumTotalPricesWithoutSpecialOffers(char[] itemsToScan, int expectedTotal)
    {
        //Arrange
        var checkout = new Checkout(_unitPrices, _specialPrices);

        foreach (var item in itemsToScan)
        {
            checkout.Scan(item);
        }

        //Act
        var result = checkout.GetTotalPrice();

        //Assert
        Assert.Equal(expectedTotal, result);
    }

    public static IEnumerable<object[]> SpecialOffersData =>
    new[]
    {
        new object[] { new char[] { 'A', 'A', 'A' }, 130 },
        new object[] { new char[] { 'A', 'A', 'A', 'A' }, 180 },
        new object[] { new char[] { 'A', 'A', 'A', 'A', 'A' }, 230 },
        new object[] { new char[] { 'A', 'A', 'A', 'A', 'A', 'A' }, 260 },
        new object[] { new char[] { 'A', 'A', 'A', 'B' }, 160 },
        new object[] { new char[] { 'A', 'A', 'A', 'B', 'B' }, 175 },
        new object[] { new char[] { 'A', 'A', 'A', 'B', 'B', 'D' }, 190 },
        new object[] { new char[] { 'D', 'A', 'B', 'A', 'B', 'A' }, 190 },
        // Add more test data here as appropriate
    };

    [Theory]
    [MemberData(nameof(SpecialOffersData))]
    public void GetTotalPrice_SumTotalPricesWithSpecialOffers(char[] itemsToScan, int expectedTotal)
    {
        //Arrange
        var checkout = new Checkout(_unitPrices, _specialPrices);

        foreach (var item in itemsToScan)
        {
            checkout.Scan(item);
        }

        //Act
        var result = checkout.GetTotalPrice();

        //Assert
        Assert.Equal(expectedTotal, result);
    }
}