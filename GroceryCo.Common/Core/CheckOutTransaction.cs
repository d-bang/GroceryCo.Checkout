using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GroceryCo.Common.Promo.Interfaces;
using GroceryCo.Common.Core.Interfaces;

namespace GroceryCo.Common.Core
{
    public class CheckOutTransaction : ICheckOutTransaction
    {
        private IList<IBatchPromotion> batchPromotions;
        private IList<ISalePromotion> salePromotions;
        private IList<IProduct> itemsForPurchase;
        private IDictionary<Guid?,ProductBag> productBags;
        private decimal amountDue;
        private string receipt;
        

        public CheckOutTransaction(IList<IBatchPromotion> BatchPromos, IList<ISalePromotion> SalePromos, IList<IProduct> ItemsForPurchase)
        {
            batchPromotions = BatchPromos;
            salePromotions = SalePromos;
            itemsForPurchase = ItemsForPurchase;
            Initialize();
            ExecuteTransaction();
        }

        public decimal AmountDue { get => amountDue; }

        public string PrintReceipt()
        {
            return receipt;
        }



        private void Initialize()
        {
            productBags = new Dictionary<Guid?,ProductBag>();

            CreateAndFillProductBags();
        }

        private void CreateAndFillProductBags()
        {
            foreach(var product in itemsForPurchase)
            {
                var isSingleItem = itemsForPurchase.Count(x => x.ID == product.ID) == 1;
                Guid? productBagId = Guid.Empty;
                IBatchPromotion batchPromo = null;
                ISalePromotion salePromo = GetSalePromo(product.ID);

                if(salePromo != null)
                {
                    product.SetSalePrice(salePromo.Saleprice);
                }
                
                if (!isSingleItem)
                {
                    batchPromo = GetBatchPromo(product.ID);
                    productBagId = product.ID;
                }
                    

                if (productBags.ContainsKey(productBagId))
                {
                    productBags[productBagId].items.Add(product);
                }
                else
                {
                    productBags.Add(productBagId, new ProductBag() { id = productBagId, batchPromotion = batchPromo, salePromotion = salePromo, items = new List<IProduct>() { product } });
                }


            }
        }

        private void ExecuteTransaction()
        {
            CalculateAmountDue();
            CreateReceipt();
        }

        private IBatchPromotion GetBatchPromo(Guid Id)
        {
            return batchPromotions.FirstOrDefault(x => x.ItemId == Id);
        }

        private ISalePromotion GetSalePromo(Guid Id)
        {
            return salePromotions.FirstOrDefault(x => x.ItemId == Id);
        }

        private void CalculateAmountDue()
        {
            foreach(var key in productBags.Keys)
            {
                var bag = productBags[key];
                amountDue += GetProductBagAmountDue(bag);
            }
        }

        private void CreateReceipt()
        {
            var builder = new StringBuilder();

            builder.AppendLine();
            builder.AppendLine("GroceryCo Receipt");
            builder.AppendLine(DateTime.Today.ToString()).AppendLine();

            foreach (var key in productBags.Keys)
            {
                var bag = productBags[key];
                builder.AppendLine(GetProductBagReceiptSection(bag));
            }

            builder.AppendLine("------------------------------").AppendLine();
            builder.AppendFormat("Amount due: ${0}", amountDue).AppendLine();

            receipt = builder.ToString();
        }

        private decimal GetProductBagAmountDue(ProductBag productBag)
        {
            var rtn = 0M;

            if (productBag.batchPromotion != null)
            {
                var price = productBag.items[0].AmountDue;
                rtn = productBag.batchPromotion.CalculateBatchPrice(price, productBag.items.Count);
            }
            else
            {
                foreach (var item in productBag.items)
                {
                    rtn += item.AmountDue;
                }
            }

            return rtn;

        }

        private string GetProductBagReceiptSection(ProductBag productBag)
        {
            var builder = new StringBuilder();
            var isBatchPromo = productBag.batchPromotion != null;

            if(isBatchPromo)
            {
                var count = productBag.items.Count;
                var amountDue = productBag.items[0].AmountDue;
                var regularPrice = productBag.items[0].Price;
                var description = productBag.items[0].Description;
                var hasItemSalePrice = productBag.items[0].AmountDue != productBag.items[0].Price;

                if (hasItemSalePrice)
                    builder.AppendFormat("Sale on items - Regularly ${0} Now ${1}", regularPrice, amountDue);

                builder.AppendFormat("{0} - {1} For ${2}", description, count, amountDue).AppendLine();
            }
            else
            {
                foreach(var item in productBag.items)
                {
                    var amountDue = item.AmountDue;
                    var regularPrice = item.Price;
                    var description = item.Description;
                    var hasItemSalePrice = item.AmountDue != item.Price;

                     builder.AppendFormat("{0} - 1 @ ${1}", description, amountDue);

                    if (hasItemSalePrice)
                        builder.AppendFormat(" Regularly ${0}", regularPrice);

                    builder.AppendLine();
                     
                }
            }
             
            return builder.ToString();
        }
    }

    public struct ProductBag
    {
        public Guid? id;
        public IList<IProduct> items;
        public IBatchPromotion batchPromotion;
        public ISalePromotion salePromotion;
    }

}
