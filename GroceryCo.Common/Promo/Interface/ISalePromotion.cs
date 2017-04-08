using GroceryCo.Common.Promo.Interfaces;

namespace GroceryCo.Common.Promo.Interfaces
{
    public interface ISalePromotion : IPromotion
    {
        decimal Saleprice { get; }
    }
}
