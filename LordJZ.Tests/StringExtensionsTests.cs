using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LordJZ.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
        static void TestUrlizeHelper(string expected, string input)
        {
            Assert.AreEqual(expected, input.Urlize());
        }

        [TestMethod]
        public void TestUrlize()
        {
            TestUrlizeHelper("", "");
            TestUrlizeHelper("", "-");
            TestUrlizeHelper("", " ");
            TestUrlizeHelper("a", "A");
            TestUrlizeHelper("a-b", "A / B");
            TestUrlizeHelper("a-b", "A : B");
            TestUrlizeHelper("a-b", "A B");
            TestUrlizeHelper("ab", "Ab");
            TestUrlizeHelper("ab", "ab ");
            TestUrlizeHelper("ab", "ab /");
            TestUrlizeHelper("ab", "/ ab /");
            TestUrlizeHelper("ab", "\\ ab \\");
            TestUrlizeHelper("a-b", ";a;b ;");
            TestUrlizeHelper("a-b", "-- ;a;b ; ---");
            TestUrlizeHelper("a-b", "/ ;a/b ; /");
        }
    }
}
