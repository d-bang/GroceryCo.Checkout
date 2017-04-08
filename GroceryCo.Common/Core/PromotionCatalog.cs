using GroceryCo.Common.Data.Interfaces;
using GroceryCo.Common.Core.Interfaces;
using GroceryCo.Common.Promo.Interfaces;
using GroceryCo.Common.Core.Base;
using System;
using System.Linq;
using System.Collections.Generic;


namespace GroceryCo.Common.Core
{
    public class PromotionCatalog : CatalogBase<IPromotion>, IPromotionCatalog
    {

        public PromotionCatalog(IDataContext dataContext) : base(dataContext)
        {

        }

        public IEnumerable<KeyValuePair<Guid, IPromotion>> GetItems(Guid id)
        {
            return catalogItems.Where(x => x.Key == id);

        }
    }
}
