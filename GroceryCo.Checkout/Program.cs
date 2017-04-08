using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Autofac;
using GroceryCo.Common.Data.Interfaces;
using GroceryCo.Common.Data;
using GroceryCo.Common.Core.Interfaces;
using GroceryCo.Common.Core;
using GroceryCo.Common.Promo.Interfaces;
using GroceryCo.Common.Promo;
using System.Threading;

namespace GroceryCo.Checkout
{
    class Program
    {
        static FilePaths pathContainer;
        static IContainer Container;
        static List<IProduct> scannedList;
        static IList<IBatchPromotion> batchPromotions;
        static IList<ISalePromotion> salePromotions;
        static IDictionary<Guid,IProduct> productCatalog;
        static IDictionary<Guid,IPromotion> promotionCatalog;

        static IList<Type> knownTypeList;

        static void Main(string[] args)
        {
            try
            {
                RegisterTypes();
                CreateKnowTypesList();

                using (var scope = Container.BeginLifetimeScope())
                {
                    while (true)
                    {
                        GetFilePaths();
                        Initialize();

                        var checkOutTrans = Container.Resolve<ICheckOutTransaction>(new NamedParameter("BatchPromos", batchPromotions),
                                                                                    new NamedParameter("SalePromos", salePromotions),
                                                                                    new NamedParameter("ItemsForPurchase", scannedList));

                        Console.WriteLine(checkOutTrans.PrintReceipt());
                        Console.WriteLine("");

                        Thread.Sleep(5000);


                    }
                }
            }
           catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.Read();
            }
                
            
            
        }

        static void GetFilePaths()
        {
            Console.WriteLine("Please enter the paths to the scanned items file, product catalog file, and the promotion catalog file in that order and comma separated.");
            Console.WriteLine("e.g. C:\\scannedItems.txt,C:\\productsCatalog.json,C:\\promotionsCatalog.json");
            Console.WriteLine("Thanks!");

            var paths = Console.ReadLine();

            var pathsArray = paths.Split(',');

            pathContainer = new FilePaths()
                                {
                                    ScannedItemsPath = pathsArray[0],
                                    ProductCatalogPath = pathsArray[1],
                                    PromotionCatalogPath = pathsArray[2]
                                };
        }

        static void CreatePromotionLists()
        {
            var distinctList = scannedList.Select(x => x.ID).Distinct();

            batchPromotions = new List<IBatchPromotion>();
            salePromotions = new List<ISalePromotion>();

            //Only get promotions related to the list of scanned items
            foreach (var item in distinctList)
            {
                var promotions = promotionCatalog.Where(x => x.Key == item);

                var salesPromos = promotions.Where(x => x.Value.GetType() == typeof(SalePromotion));
                var batchPromos = promotions.Where(x => x.Value.GetType() == typeof(BatchPromotion)).ToList();

                foreach(var salesPromo in salesPromos)
                {
                    salePromotions.Add(salesPromo.Value as ISalePromotion);
                }

                foreach(var batchPromo in batchPromos)
                {
                    batchPromotions.Add(batchPromo.Value as IBatchPromotion);
                }
                
            }
            
        }

        static void LoadFilesIntoObjects()
        {
            LoadProductCatalog();
            LoadPromotionCatalog();
            LoadScannedItemList();
        }

        static void LoadProductCatalog()
        {
            var productCatalogAccess = Container.Resolve<IDataContext>(new NamedParameter("filePath", pathContainer.ProductCatalogPath),
                                                                       new NamedParameter("knownTypes", knownTypeList));

            productCatalog = productCatalogAccess.LoadData<IDictionary<Guid, IProduct>>();
        }

        static void LoadPromotionCatalog()
        {
            var promotionCatalogAccess = Container.Resolve<IDataContext>(new NamedParameter("filePath", pathContainer.PromotionCatalogPath),
                                                                         new NamedParameter("knownTypes", knownTypeList));

            promotionCatalog = promotionCatalogAccess.LoadData<IDictionary<Guid, IPromotion>>();
        }

        static void LoadScannedItemList()
        {
            scannedList = new List<IProduct>();

            string itemsText = File.ReadAllText(pathContainer.ScannedItemsPath);
            var items = itemsText.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach(var item in items)
            {
                var key = new Guid(item);
                var product = productCatalog[key];
                scannedList.Add(product);
            }

        }

        static void RegisterTypes()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<JSonFileDataContext>().As<IDataContext>();
            builder.RegisterType<CheckOutTransaction>().As<ICheckOutTransaction>();
            builder.RegisterType<SalePromotion>().As<ISalePromotion>();
            builder.RegisterType<BatchPromotion>().As<IBatchPromotion>();
            builder.RegisterType<BogoDiscountCalculator>().As<IBogoCalculator>();
            builder.RegisterType<GroupDiscountCalculator>().As<IGroupCalculator>();
            builder.RegisterType<Product>().As<IProduct>();
            builder.RegisterType<ProductCatalog>().As<IProductCatalog>();
            builder.RegisterType<PromotionCatalog>().As<IPromotionCatalog>();
            builder.RegisterType<CheckOutTransaction>().As<ICheckOutTransaction>();


            Container = builder.Build();
        }

        static void CreateKnowTypesList()
        {
            knownTypeList = new List<Type>();
            knownTypeList.Add(typeof(Product));
            knownTypeList.Add(typeof(BatchPromotion));
            knownTypeList.Add(typeof(SalePromotion));
            knownTypeList.Add(typeof(BogoDiscountCalculator));
            knownTypeList.Add(typeof(GroupDiscountCalculator));
        }

        static void Initialize()
        {
            LoadFilesIntoObjects();
            CreatePromotionLists();

        }
    }

    public struct FilePaths
    {
        public string ScannedItemsPath;
        public string ProductCatalogPath;
        public string PromotionCatalogPath;

    }

}
