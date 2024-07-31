namespace CheckoutLibrary.Interfaces;

public interface ICheckout
{
    public void Scan(char item) =>
        throw new NotImplementedException("Scan function has not been implemented");

    public int GetTotalPrice() =>
        throw new NotImplementedException("TotalPrice function hasn't been implemented");
}
