using System;
using GroceryCo.Common.Promo.Base;
using GroceryCo.Common.Promo.Interfaces;
using Newtonsoft.Json;

namespace GroceryCo.Common.Promo
{
    public class SalePromotion : PromotionBase, ISalePromotion
    {
        [JsonProperty]
        private decimal salePrice;

        public SalePromotion(DateTime start, DateTime end, Guid itemId, decimal SalePrice, string description) : base(start,end,itemId,description)
        {
            salePrice = SalePrice;
        }

        public decimal Saleprice { get => salePrice; }   
    }
}
