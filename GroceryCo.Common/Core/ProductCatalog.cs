using GroceryCo.Common.Data.Interfaces;
using GroceryCo.Common.Core.Interfaces;
using GroceryCo.Common.Core.Base;


namespace GroceryCo.Common.Core
{
    public class ProductCatalog : CatalogBase<IProduct>, IProductCatalog
    {
        
        public ProductCatalog(IDataContext dataContext) : base(dataContext)
        {

        }
 
    }
}
