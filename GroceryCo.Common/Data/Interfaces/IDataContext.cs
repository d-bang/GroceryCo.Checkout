using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Common.Data.Interfaces
{
    public interface IDataContext
    {
        T LoadData<T>();
        void SaveData<T>(T data);
    }
}
