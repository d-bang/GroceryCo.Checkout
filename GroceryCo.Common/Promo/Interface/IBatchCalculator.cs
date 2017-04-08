namespace GroceryCo.Common.Promo.Interfaces
{
    public interface IBatchCalculator
    {
        decimal CalculateBatchPrice(decimal price, int itemCount);
    }
}
