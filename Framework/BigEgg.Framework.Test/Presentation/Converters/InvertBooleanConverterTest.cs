using BigEgg.Framework.Presentation.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigEgg.Framework.Test.Presentation.Converters
{
    [TestClass]
    public class InvertBooleanConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            var converter = InvertBooleanConverter.Default;
            Assert.IsFalse((bool)converter.Convert(true, null, null, null));
            Assert.IsTrue((bool)converter.Convert(false, null, null, null));

            Assert.IsFalse((bool)converter.ConvertBack(true, null, null, null));
            Assert.IsTrue((bool)converter.ConvertBack(false, null, null, null));
        }
    }
}
