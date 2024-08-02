namespace CheckoutLibrary.Models;

public class SpecialPrice
{
    public int _setAmount {  get; private set; }
    public int _setPrice { get; private set; }

    public SpecialPrice(int setAmount, int setPrice)
    {
        _setAmount = setAmount;
        _setPrice = setPrice;
    }
}
