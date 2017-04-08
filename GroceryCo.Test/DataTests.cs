using System;
using System.IO;
using System.Collections.Generic;
using GroceryCo.Common.Data;
using GroceryCo.Common.Core;
using GroceryCo.Common.Core.Interfaces;
using GroceryCo.Common.Promo;
using GroceryCo.Common.Promo.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GroceryCo.Test
{
    [TestClass]
    public class DataTests
    {
        static IList<Type> knownTypes;
        static string dir = @"C:\temp\testing\Data";
        static string productCatalogFile = @"ProductCatalog.Json";
        static string promotionCatalogFile = @"PromotionCatalog.Json";
        static string productCatalogPath;
        static string promotionCatalogPath;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            knownTypes = new List<Type>
                            {
                                typeof(Product),
                                typeof(BatchPromotion),
                                typeof(SalePromotion),
                                typeof(BogoDiscountCalculator),
                                typeof(GroupDiscountCalculator)
                            };

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            productCatalogPath = Path.Combine(dir,productCatalogFile);
            promotionCatalogPath = Path.Combine(dir,promotionCatalogFile);

            RemoveCatalogFiles();
        }

        Dictionary<Guid, IProduct> CreateProductCatalog()
        {
            var item1 = new Product(0.33M, Guid.NewGuid(), "Banana");
            var item2 = new Product(1.00M, Guid.NewGuid(), "Gala Apple");
            var item3 = new Product(4.99M, Guid.NewGuid(), "Honey 500 ml");
            var item4 = new Product(8.55M, Guid.NewGuid(), "Cheese 500 g");
            var item5 = new Product(7.79M, Guid.NewGuid(), "Toasty O's Cereal 900 g");

            return new Dictionary<Guid, IProduct>
                        {
                            { item1.ID, item1 },
                            { item2.ID, item2 },
                            { item3.ID, item3 },
                            { item4.ID, item4 },
                            { item5.ID, item5 }
                        };

            
        }
        Dictionary<Guid, IPromotion> CreatePromotionCatalog()
        {
            var promo1 = new SalePromotion(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1), Guid.Parse("e1d57fe0-74b0-4d92-8b76-1e895ea563d4"), 0.50M, "Gala Apple Sale");
            var promo2 = new BatchPromotion(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1), Guid.Parse("640d4eb6-f537-4a22-9690-15792f6509f9"), "Honey - Buy 1 get on 1/2 off Sale", new BogoDiscountCalculator(1, 0.5M));
            var promo3 = new BatchPromotion(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1), Guid.Parse("ff9a9441-8a08-4ddd-9009-d7d5222434bc"), "Bunch 'O' cheese Sale", new GroupDiscountCalculator(3, 8.55M));
            var promo4 = new SalePromotion(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), Guid.Parse("ad04ff83-5f0b-4092-ae69-d432c3446821"), 0.17M, "Banana Sale");

            return new Dictionary<Guid, IPromotion>
                        {
                            { promo1.ItemId, promo1 },
                            { promo2.ItemId, promo2 },
                            { promo3.ItemId, promo3 },
                            { promo4.ItemId, promo4 }
                        };

           
        }
        void CreateProductCatalogFile(IDictionary<Guid, IProduct> dict)
        {
            var dataContext = new JSonFileDataContext(productCatalogPath, knownTypes);
            dataContext.SaveData(dict);
        }
        void CreatePromotionCatalogFile(IDictionary<Guid, IPromotion> dict)
        {
            var dataContext = new JSonFileDataContext(promotionCatalogPath, knownTypes);
            dataContext.SaveData(dict);
        }
        static void RemoveCatalogFiles()
        {
            if (File.Exists(productCatalogPath))
                File.Delete(productCatalogPath);

            if (File.Exists(promotionCatalogPath))
                File.Delete(promotionCatalogPath);
        }

        [TestCleanup]
        public void Cleanup()
        {
            RemoveCatalogFiles();
        }

        [TestMethod]
        public void SaveFullProductCatalogTest()
        {
            try
            {
                Assert.IsTrue(!File.Exists(productCatalogPath));
                var dict = CreateProductCatalog();
                CreateProductCatalogFile(dict);

                Assert.IsTrue(File.Exists(productCatalogPath));
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.ToString());
            }

        }

        [TestMethod]
        public void SaveEmptyProductCatalogTest()
        {
            try
            {
                Assert.IsTrue(!File.Exists(productCatalogPath));
                var dict = new Dictionary<Guid, IProduct>();
                CreateProductCatalogFile(dict);

                Assert.IsTrue(File.Exists(productCatalogPath));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }

        }

        [TestMethod]
        public void SaveFullPromotionCatalogTest()
        {
            try
            {
                Assert.IsTrue(!File.Exists(promotionCatalogPath));
                var dict = CreatePromotionCatalog();
                CreatePromotionCatalogFile(dict);

                Assert.IsTrue(File.Exists(promotionCatalogPath));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }

        }

        [TestMethod]
        public void SaveEmptyPromotionCatalogTest()
        {
            try
            {
                Assert.IsTrue(!File.Exists(promotionCatalogPath));
                var dict = new Dictionary<Guid, IPromotion>();
                CreatePromotionCatalogFile(dict);

                Assert.IsTrue(File.Exists(promotionCatalogPath));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }

        }

        [TestMethod]
        public void ReadFullProductCatalogTest()
        {
            try
            {
                var writeDict = CreateProductCatalog();
                CreateProductCatalogFile(writeDict);

                Assert.IsTrue(File.Exists(productCatalogPath));

                var dataContext = new JSonFileDataContext(productCatalogPath, knownTypes);
                var readDict = dataContext.LoadData<IDictionary<Guid, IProduct>>();

                Assert.IsNotNull(readDict);
                Assert.AreEqual(readDict.Count, 5);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void ReadFullPromotionCatalogTest()
        {
            try
            {
                var writeDict = CreatePromotionCatalog();
                CreatePromotionCatalogFile(writeDict);

                Assert.IsTrue(File.Exists(promotionCatalogPath));

                var dataContext = new JSonFileDataContext(promotionCatalogPath, knownTypes);
                var readDict = dataContext.LoadData<IDictionary<Guid, IPromotion>>();

                Assert.IsNotNull(readDict);
                Assert.AreEqual(readDict.Count, 4);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void ReadEmptyProductCatalogTest()
        {
            try
            {
                var writeDict = new Dictionary<Guid, IProduct>();
                CreateProductCatalogFile(writeDict);

                Assert.IsTrue(File.Exists(productCatalogPath));

                var dataContext = new JSonFileDataContext(productCatalogPath, knownTypes);
                var readDict = dataContext.LoadData<IDictionary<Guid, IProduct>>();

                Assert.IsNotNull(readDict);
                Assert.AreEqual(readDict.Count, 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void ReadEmptyPromotionCatalogTest()
        {
            try
            {
                var writeDict = new Dictionary<Guid, IPromotion>();
                CreatePromotionCatalogFile(writeDict);

                Assert.IsTrue(File.Exists(promotionCatalogPath));

                var dataContext = new JSonFileDataContext(promotionCatalogPath, knownTypes);
                var readDict = dataContext.LoadData<IDictionary<Guid, IPromotion>>();

                Assert.IsNotNull(readDict);
                Assert.AreEqual(readDict.Count, 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void FailSaveProductCatalogTest()
        {
            try
            {
                var dict = new Dictionary<Guid,IProduct>();
                var dataContext = new JSonFileDataContext("", knownTypes);
                dataContext.SaveData(dict);

                Assert.Fail("File Path is invalid");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex != null);
            }
        }

        [TestMethod]
        public void FailSavePromotionCatalogTest()
        {
            try
            {
                var dict = new Dictionary<Guid,IPromotion>();
                var dataContext = new JSonFileDataContext("", knownTypes);
                dataContext.SaveData(dict);

                Assert.Fail("File Path is invalid");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex != null);
            }
        }
    }
}
