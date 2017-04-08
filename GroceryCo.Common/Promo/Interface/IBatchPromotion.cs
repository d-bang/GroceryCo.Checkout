

namespace GroceryCo.Common.Promo.Interfaces
{
    public interface IBatchPromotion : IPromotion
    {
        decimal CalculateBatchPrice(decimal itemPrice, int itemCount);
    }
}
