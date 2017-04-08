using System;

namespace GroceryCo.Common.Core.Interfaces
{
    public interface IProduct
    {
        decimal Price { get; }
        decimal AmountDue { get; }
        void SetSalePrice(decimal value);
        Guid ID { get; }
        string Description { get; }
    }
}
