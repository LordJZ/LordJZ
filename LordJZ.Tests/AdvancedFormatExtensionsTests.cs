using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LordJZ.Tests
{
    [TestClass]
    public class AdvancedFormatExtensionsTests
    {
        void TestRegularFormatHelper(IFormatProvider provider, string format, params object[] args)
        {
            Assert.AreEqual(
                string.Format(provider, format, args),
                format.AdvancedFormat(provider, args)
                );
        }

        [TestMethod]
        public void TestRegularFormat()
        {
            this.TestRegularFormatHelper(null, "{{{0,-20:N}}}", 1);
            this.TestRegularFormatHelper(null, "{{{0,20:N}}}", int.MinValue);
            this.TestRegularFormatHelper(null, "{{{0,-20:N}}}", int.MaxValue);

            this.TestRegularFormatHelper(null, "{{{0,20}}}", DateTime.Now);

            this.TestRegularFormatHelper(null, "{{{0,20}}}", true);

            this.TestRegularFormatHelper(null, "{{{0,20}}}", new object[] { null });
        }

        void TestBoolHelper(string expected, string format, params object[] args)
        {
            Assert.AreEqual(
                expected,
                format.AdvancedFormat(args)
                );
        }

        [TestMethod]
        public void TestBool()
        {
            this.TestBoolHelper("one", "{0?bool:one:two}", 1);
            this.TestBoolHelper("two", "{0?bool:one:two}", 0);
            this.TestBoolHelper("two", "{0?bool:one:two}");

            this.TestBoolHelper("two", "{0?bool:one:two}", "");
            this.TestBoolHelper("two", "{0?bool:one:two}", (string)null);
            this.TestBoolHelper("two", "{0?bool:one:two}", "false");
            this.TestBoolHelper("two", "{0?bool:one:two}", "0");
            this.TestBoolHelper("one", "{0?bool:one:two}", "1");
            this.TestBoolHelper("one", "{0?bool:one:two}", "true");
            this.TestBoolHelper("one", "{0?bool:one:two}", "sdsdsds");

            this.TestBoolHelper("one", "{0?bool:one:two}", true);
            this.TestBoolHelper("two", "{0?bool:one:two}", false);

            this.TestBoolHelper("one", "{0?bool:one:two}", ulong.MaxValue);
            this.TestBoolHelper("two", "{0?bool:one:two}", ulong.MinValue);

            this.TestBoolHelper("one", "{0?bool:one:two}", long.MaxValue);
            this.TestBoolHelper("one", "{0?bool:one:two}", long.MinValue);
        }

        void TestEnOrdinalHelper(string expectedPostfix, long value)
        {
            Assert.AreEqual(value + expectedPostfix,
                            "{0}{0?en-ordinal}".AdvancedFormat(value));
        }

        void TestEnOrdinalHelper(string expectedPostfix, ulong value)
        {
            Assert.AreEqual(value + expectedPostfix,
                            "{0}{0?en-ordinal}".AdvancedFormat(value));
        }

        [TestMethod]
        public void TestEnOrdinal()
        {
            this.TestEnOrdinalHelper("", 1 - 2);
            this.TestEnOrdinalHelper("", 0);
            this.TestEnOrdinalHelper("st", 1);
            this.TestEnOrdinalHelper("nd", 2);
            this.TestEnOrdinalHelper("rd", 3);
            this.TestEnOrdinalHelper("th", 4);
            this.TestEnOrdinalHelper("th", 5);
            this.TestEnOrdinalHelper("th", 6);
            this.TestEnOrdinalHelper("th", 7);
            this.TestEnOrdinalHelper("th", 8);
            this.TestEnOrdinalHelper("th", 9);
            this.TestEnOrdinalHelper("th", 10);
            this.TestEnOrdinalHelper("th", 11);
            this.TestEnOrdinalHelper("th", 12);
            this.TestEnOrdinalHelper("th", 13);
            this.TestEnOrdinalHelper("th", 14);
            this.TestEnOrdinalHelper("th", 20);
            this.TestEnOrdinalHelper("st", 21);
            this.TestEnOrdinalHelper("nd", 22);
            this.TestEnOrdinalHelper("rd", 23);
            this.TestEnOrdinalHelper("th", 24);
            this.TestEnOrdinalHelper("th", 100);
            this.TestEnOrdinalHelper("st", 101);
            this.TestEnOrdinalHelper("nd", 102);
            this.TestEnOrdinalHelper("rd", 103);
            this.TestEnOrdinalHelper("th", 104);
            this.TestEnOrdinalHelper("th", 110);
            this.TestEnOrdinalHelper("th", 111);
            this.TestEnOrdinalHelper("th", 112);
            this.TestEnOrdinalHelper("th", 113);
            this.TestEnOrdinalHelper("th", 114);
            this.TestEnOrdinalHelper("th", 120);
            this.TestEnOrdinalHelper("st", 121);
            this.TestEnOrdinalHelper("nd", 122);
            this.TestEnOrdinalHelper("rd", 123);
            this.TestEnOrdinalHelper("th", 124);
            this.TestEnOrdinalHelper("", long.MinValue);
            this.TestEnOrdinalHelper("th", long.MaxValue);
            this.TestEnOrdinalHelper("", ulong.MinValue);
            this.TestEnOrdinalHelper("th", ulong.MaxValue);
        }
    }
}
