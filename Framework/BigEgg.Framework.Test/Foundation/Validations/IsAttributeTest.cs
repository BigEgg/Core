using BigEgg.Framework.Foundation;
using BigEgg.Framework.Foundation.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;

namespace BigEgg.Framework.Test.Foundation.Validations
{
    [TestClass]
    public class IsAttributeTest
    {
        [TestMethod]
        public void ValidationTest()
        {
            SomeClass testSubject = new SomeClass();

            Assert.AreEqual(string.Empty, testSubject.Validate("TestValue1"));
            Assert.AreNotEqual(string.Empty, testSubject.Validate("TestValue2"));
            Assert.AreNotEqual(string.Empty, testSubject.Validate("TestProperty"));
            Assert.AreNotEqual(string.Empty, testSubject.Validate());

            //  Validation by value
            testSubject.TestValue1 = "abc";
            Assert.AreEqual(string.Empty, testSubject.Validate("TestValue1"));

            testSubject.TestValue2 = "1";
            Assert.AreNotEqual(string.Empty, testSubject.Validate("TestValue2"));

            //  Validation by property
            testSubject.DepentdentProperty = "abc";
            testSubject.TestProperty = "abcd";
            Assert.AreNotEqual(string.Empty, testSubject.Validate("TestProperty"));
            testSubject.TestProperty = "abc";
            Assert.AreEqual(string.Empty, testSubject.Validate("TestProperty"));
        }

        private class SomeClass : IDataErrorInfo
        {
            [NonSerialized]
            private readonly DataErrorInfoSupport dataErrorInfoSupport;

            string IDataErrorInfo.Error { get { return this.dataErrorInfoSupport.Error; } }

            string IDataErrorInfo.this[string memberName] { get { return this.dataErrorInfoSupport[memberName]; } }


            public SomeClass()
            {
                this.dataErrorInfoSupport = new DataErrorInfoSupport(this);
            }


            public string DepentdentProperty { get; set; }

            [Is("abc", PassOnNull = true, ErrorMessage = "Error1")]
            public string TestValue1 { get; set; }

            [Is(1, ErrorMessage = "Error2")]
            public string TestValue2 { get; set; }

            [Is("DepentdentProperty", ValidationType.ByProperty, ErrorMessage = "Error3")]
            public string TestProperty { get; set; }
        }
    }
}
