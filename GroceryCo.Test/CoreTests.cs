using System;
using System.IO;
using System.Collections;
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
    public class CoreTests
    {
        static IList<Type> knownTypes;
        static string dir = @"C:\temp\testing\Core";
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

            productCatalogPath = Path.Combine(dir, productCatalogFile);
            promotionCatalogPath = Path.Combine(dir, promotionCatalogFile);

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
        List<IProduct> CreateProductList()
        {
            var list = new List<IProduct>();
            list.Add(new Product(10M, Guid.NewGuid(), "Test1"));
            list.Add(new Product(5M, Guid.NewGuid(), "Test2"));
            list.Add(new Product(20M, Guid.NewGuid(), "Test3"));

            return list;
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
        public void PassProductInitializeTest()
        {
            try
            {
                var price = 20M;
                var id = Guid.NewGuid();
                var descrip = "Test";

                var product = new Product(price, id, descrip);

                Assert.IsNotNull(product);
                Assert.AreEqual(price, product.Price);
                Assert.AreEqual(id, product.ID);
                Assert.AreEqual(descrip, product.Description);
                Assert.AreEqual(price, product.AmountDue);
                Assert.AreEqual(price, product.Price);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void PassProductSaleTest()
        {
            try
            {
                var price = 20M;
                var salePrice = 10M;
                var id = Guid.NewGuid();
                var descrip = "Test";

                var product = new Product(price, id, descrip);

                Assert.AreEqual(price, product.AmountDue);

                product.SetSalePrice(salePrice);

                Assert.AreEqual(salePrice, product.AmountDue);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void PassProductCatalogInitTest()
        {
            try
            {
                CreateProductCatalogFile(CreateProductCatalog());
                var dataContext = new JSonFileDataContext(productCatalogPath, knownTypes);

                var productCatalog = new ProductCatalog(dataContext);

                productCatalog.LoadCatalog();

                Assert.IsNotNull(productCatalog);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void PassProductCatalogLoadFullTest()
        {
            try
            {
                var virtualCatalog = CreateProductCatalog();
                CreateProductCatalogFile(virtualCatalog);
                var dataContext = new JSonFileDataContext(productCatalogPath, knownTypes);

                var productCatalog = new ProductCatalog(dataContext);

                foreach (var key in virtualCatalog.Keys)
                {
                    Assert.IsNotNull(productCatalog.ItemExists(key));
                }

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void PassProductCatalogSaveFullTest()
        {
            try
            {
                var virtualCatalog = CreateProductCatalog();
                CreateProductCatalogFile(virtualCatalog);
                var dataContext = new JSonFileDataContext(productCatalogPath, knownTypes);

                var productCatalog = new ProductCatalog(dataContext);

                File.Delete(productCatalogPath);

                Assert.IsTrue(!File.Exists(productCatalogPath));

                productCatalog.SaveCatalog(virtualCatalog);

                Assert.IsTrue(File.Exists(productCatalogPath));

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void PassPromotionCatalogInitTest()
        {
            try
            {
                CreatePromotionCatalogFile(CreatePromotionCatalog());
                var dataContext = new JSonFileDataContext(promotionCatalogPath, knownTypes);

                var promotionCatalog = new PromotionCatalog(dataContext);

                promotionCatalog.LoadCatalog();

                Assert.IsNotNull(promotionCatalog);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void PassPromtionCatalogLoadFullTest()
        {
            try
            {
                var virtualCatalog = CreatePromotionCatalog();
                CreatePromotionCatalogFile(virtualCatalog);
                var dataContext = new JSonFileDataContext(promotionCatalogPath, knownTypes);

                var promotionCatalog = new PromotionCatalog(dataContext);

                foreach (var key in virtualCatalog.Keys)
                {
                    Assert.IsNotNull(promotionCatalog.ItemExists(key));
                }

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void PassPromotionCatalogSaveFullTest()
        {
            try
            {
                var virtualCatalog = CreatePromotionCatalog();
                CreatePromotionCatalogFile(virtualCatalog);
                var dataContext = new JSonFileDataContext(promotionCatalogPath, knownTypes);

                var promotionCatalog = new PromotionCatalog(dataContext);

                File.Delete(promotionCatalogPath);

                Assert.IsTrue(!File.Exists(promotionCatalogPath));

                promotionCatalog.SaveCatalog(virtualCatalog);

                Assert.IsTrue(File.Exists(promotionCatalogPath));

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void PassCheckOutTransactionInitTest()
        {
            try
            {
                var chkOutTrans = new CheckOutTransaction(new List<IBatchPromotion>(), new List<ISalePromotion>(), new List<IProduct>());

                Assert.IsNotNull(chkOutTrans);
                Assert.AreEqual(0, chkOutTrans.AmountDue);
                Assert.IsTrue(chkOutTrans.PrintReceipt().Length > 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }


        }

        [TestMethod]
        public void PassCheckOutTransactionTest()
        {
            try
            {
                var chkOutTrans = new CheckOutTransaction(new List<IBatchPromotion>(), new List<ISalePromotion>(), CreateProductList());

                Assert.IsNotNull(chkOutTrans);
                Assert.AreEqual(35, chkOutTrans.AmountDue);
                Assert.IsTrue(chkOutTrans.PrintReceipt().Length > 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }


        }

        
    }
}

