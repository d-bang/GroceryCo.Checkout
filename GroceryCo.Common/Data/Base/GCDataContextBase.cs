using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryCo.Common.Data.Interfaces;

namespace GroceryCo.Common.Data.Base
{
    public abstract class GCDataContextBase<T>
    {
        protected T StorageAccessParameter;

        public GCDataContextBase(T storageAccessParam)
        {
            StorageAccessParameter = storageAccessParam;
        }

    }
}
