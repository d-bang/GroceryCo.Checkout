using System;
using GroceryCo.Common.Core.Interfaces;
using Newtonsoft.Json;

namespace GroceryCo.Common.Core
{
    public class Product : IProduct
    {
        [JsonProperty]
        private decimal? salePrice = null;
        [JsonProperty]
        private decimal price;
        [JsonProperty]
        private Guid id;
        [JsonProperty]
        private string description;
        

        public Product(decimal regularPrice, Guid identifier, string shortDescription)
        {
            price = regularPrice;
            id = identifier;
            description = shortDescription;
        }

        public decimal Price { get => price; }
        public decimal AmountDue { get { return GetPrice(); } }
        public Guid ID { get => id; }
        public string Description { get => description; }

        

        public void SetSalePrice(decimal price)
        {
            salePrice = price;
        }

        private decimal GetPrice()
        {
            return salePrice.HasValue ? salePrice.Value : price;
        }
    }
}
