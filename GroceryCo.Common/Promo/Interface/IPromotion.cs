using System;

namespace GroceryCo.Common.Promo.Interfaces
{
    public interface IPromotion
    {
        Guid ItemId { get; }
        DateTime StartDate { get; }
        DateTime EndDate { get; }
        String Description { get; }

    }
}
