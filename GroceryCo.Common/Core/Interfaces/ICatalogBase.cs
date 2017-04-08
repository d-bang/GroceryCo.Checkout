using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Common.Core.Interfaces
{
    public interface ICatalogBase<T>
    {
        void LoadCatalog();
        void SaveCatalog(IDictionary<Guid, T> items);
        bool ItemExists(Guid id);
        T GetItem(Guid id);
    }
}
