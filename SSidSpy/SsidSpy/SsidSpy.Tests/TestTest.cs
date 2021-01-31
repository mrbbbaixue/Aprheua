using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SsidSpy.Tests
{
    [TestClass]
    public class TestTest
    {
        [TestMethod]
        public void 必须通过测试()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void 杯子被喝空了吗()
        {
            var 我的水杯 = new SsidSpy.Models.杯子 {
                杯子类型 = 1,
                杯子颜色 = Models.颜色.粉色
            };
            for (int i = 1; i <= 10; i++)
            {
                我的水杯.被喝一口();
            }
            Assert.AreEqual(我的水杯.水量(),0);
        }
    }
}
    
