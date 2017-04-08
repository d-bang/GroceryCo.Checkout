using System;
using GroceryCo.Common.Promo.Interfaces;
using GroceryCo.Common.Promo.Base;
using Newtonsoft.Json;

namespace GroceryCo.Common.Promo
{
    public class BogoDiscountCalculator : BatchCalculatorBase, IBogoCalculator
    {
        [JsonProperty]
        private decimal equivalentModifier;

        public BogoDiscountCalculator(int purchaseNum, decimal equivalentMod)
        {
            PurchaseNumber = purchaseNum;

            //const decimal RANGE_MIN = 0.01M;
            //const decimal RANGE_MAX = 1.0M;

            //if (equivalentMod > RANGE_MAX || equivalentMod < RANGE_MIN)
            //    throw new ArgumentOutOfRangeException(string.Format("The value entered for equivalentMod was outside the range of accepted values [{0}-{1}]", RANGE_MIN, RANGE_MAX));

            equivalentModifier = equivalentMod;
        }

        //The discount is triggered by purchasing this many items
        public int PurchaseNumber { get; }
        public decimal EquivalentModifier
        {
            get => equivalentModifier;
        }

        /*
         * The intent of this method is to figure out how many items in the item count
         * qualify for the by x get x off discount
         * we calculate full price for half of the items in the qualified group + any in the remainder group
         * and add that to the discounted price calculation of the other half of the qualified group
         */
        public decimal CalculateBatchPrice(decimal price, int itemCount)
        {
            const int DIVIDE_BY = 2;
            const int MULTIPLY_BY = 2;

            decimal total = 0;

            var denominator = PurchaseNumber * MULTIPLY_BY; 
            var remainder = itemCount % denominator;
            var fullPriceCount = ((itemCount - remainder) / DIVIDE_BY) + remainder;
            var discountCount = (itemCount - remainder) / DIVIDE_BY;

            var fullPriceAmount = price * fullPriceCount;
            var discountAmount = (price * EquivalentModifier) * discountCount;

            total = fullPriceAmount + discountAmount;

            return total;
        }
    }
}
