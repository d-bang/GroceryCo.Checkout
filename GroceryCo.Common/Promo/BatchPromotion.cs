using System;
using GroceryCo.Common.Promo.Interfaces;
using GroceryCo.Common.Promo.Base;
using Newtonsoft.Json;

namespace GroceryCo.Common.Promo
{
    public class BatchPromotion : PromotionBase, IBatchPromotion
    {
        [JsonProperty]
        private IBatchCalculator batchCalculator;

        public BatchPromotion(DateTime start, DateTime end, Guid itemId, string description, IBatchCalculator calculator) : base(start,end,itemId,description)
        {
            batchCalculator = calculator;
        }

        public decimal CalculateBatchPrice(decimal itemPrice, int itemCount)
        {
            return batchCalculator.CalculateBatchPrice(itemPrice, itemCount);
        }
     
    }
}