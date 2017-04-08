using System;
using System.Collections.Generic;
using System.Linq;
using GroceryCo.Common.Data.Interfaces;
using GroceryCo.Common.Core.Interfaces;


namespace GroceryCo.Common.Core.Base
{
    public abstract class CatalogBase<T> : ICatalogBase<T>
    {
        protected IDataContext DataContext;
        protected IDictionary<Guid, T> catalogItems;

        public CatalogBase(IDataContext dataContext)
        {
            DataContext = dataContext;
            LoadCatalog();
        }

        public bool ItemExists(Guid id)
        {
            return catalogItems.Any(x => x.Key == id);
        }

        public void LoadCatalog()
        {
            catalogItems = DataContext.LoadData<IDictionary<Guid, T>>();
        }

        public void SaveCatalog(IDictionary<Guid, T> items)
        {
            DataContext.SaveData(items);
        }

        public T GetItem(Guid id)
        {
            return catalogItems[id];
        }
    }
}
