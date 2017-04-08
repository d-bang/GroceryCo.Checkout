using System;
using GroceryCo.Common.Promo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GroceryCo.Test
{
    [TestClass]
    public class PromoTests
    {
        [TestMethod]
        public void PassBatchInitializationTest()
        {
            try
            {
                var start = DateTime.Today;
                var end = DateTime.Today.AddDays(1);
                var id = Guid.NewGuid();
                var calc = new BogoDiscountCalculator(0, 0);

                var batchPromo = new BatchPromotion(start, end, id, "Test", calc);

                Assert.IsNotNull(batchPromo);
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void PassBatchBogoDiscountTest()
        {
            try
            {
                var start = DateTime.Today;
                var end = DateTime.Today.AddDays(1);
                var id = Guid.NewGuid();
                var calc = new BogoDiscountCalculator(1, 0.5M);

                var batchPromo = new BatchPromotion(start, end, id, "Test", calc);
                var actual =  batchPromo.CalculateBatchPrice(10, 2);

                Assert.AreEqual(15, actual);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void PassBatchGroupDiscountTest()
        {
            try
            {
                var start = DateTime.Today;
                var end = DateTime.Today.AddDays(1);
                var id = Guid.NewGuid();
                var calc = new GroupDiscountCalculator(3, 10M);

                var batchPromo = new BatchPromotion(start, end, id, "Test", calc);
                var actual = batchPromo.CalculateBatchPrice(15, 3);

                Assert.AreEqual(15, actual);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void PassSalePromoInitializeTest()
        {
            try
            {
                var start = DateTime.Today;
                var end = DateTime.Today.AddDays(1);
                var id = Guid.NewGuid();
                var salePrice = 10M;
                var descrip = "Test";

                var salePromo = new SalePromotion(start, end, id, salePrice, descrip);
               
                Assert.IsNotNull(salePromo);
                Assert.AreEqual(start, salePromo.StartDate);
                Assert.AreEqual(end, salePromo.EndDate);
                Assert.AreEqual(id, salePromo.ItemId);
                Assert.AreEqual(salePrice, salePromo.Saleprice);
                Assert.AreEqual(descrip, salePromo.Description);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }
    }
}
