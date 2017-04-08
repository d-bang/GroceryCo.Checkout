using GroceryCo.Common.Promo.Interfaces;
using GroceryCo.Common.Promo.Base;
using Newtonsoft.Json;

namespace GroceryCo.Common.Promo
{
    public class GroupDiscountCalculator : BatchCalculatorBase, IGroupCalculator
    {
        [JsonProperty]
        private int groupNumber;
        [JsonProperty]
        private decimal itemRegularPrice;


        public GroupDiscountCalculator(int GroupDiscountNumber, decimal ItemRegularPrice)
        {
            groupNumber = GroupDiscountNumber;
            itemRegularPrice = ItemRegularPrice;
        }

        public int GroupNumber { get => groupNumber; }

        /*
         * The intent of this method is to figure out how many items in the item count
         * qualify for the group discount
         * we calculate full price for the items not in the qualified group then add that to 
         * the batch cost for each qualified discount group
         */
        public decimal CalculateBatchPrice(decimal price, int itemCount)
        {     
            decimal total = 0;

            var remainder = itemCount % GroupNumber;
            var discountPriceCount = 0;
            var discountAmount = 0.0M;

            if (remainder < itemCount)
            {
                discountPriceCount = ((itemCount - remainder) / GroupNumber);
                discountAmount = price * discountPriceCount;
            }
            else
            {
                //remainder == itemcount
                var regularPrice = remainder * itemRegularPrice;

                discountAmount = (regularPrice < price) ? regularPrice : price;
            }

            total = discountAmount;

            return total;
        }
    }
}
