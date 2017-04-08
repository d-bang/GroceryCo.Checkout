using System;
using System.Collections.Generic;
using GroceryCo.Common.Promo.Interfaces;

namespace GroceryCo.Common.Core.Interfaces
{
    public interface IPromotionCatalog : ICatalogBase<IPromotion>
    {
        IEnumerable<KeyValuePair<Guid,IPromotion>> GetItems(Guid id);
    }
}
