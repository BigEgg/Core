using BigEgg.Framework.Foundation;
using BigEgg.Framework.Foundation.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;

namespace BigEgg.Framework.Test.Foundation.Validations
{
    [TestClass]
    public class GreaterThanOrEqualToAttributeTest
    {
        [TestMethod]
        public void ValidationTest()
        {
            SomeClass testSubject = new SomeClass();

            //  Default int is 0
            Assert.AreNotEqual(string.Empty, testSubject.Validate("TestValue"));
            Assert.AreEqual(string.Empty, testSubject.Validate("TestProperty"));
            Assert.AreNotEqual(string.Empty, testSubject.Validate());

            //  Validation by value
            testSubject.TestValue = 1;
            Assert.AreNotEqual(string.Empty, testSubject.Validate("TestValue"));
            testSubject.TestValue = 5;
            Assert.AreEqual(string.Empty, testSubject.Validate("TestValue"));
            testSubject.TestValue = 6;
            Assert.AreEqual(string.Empty, testSubject.Validate("TestValue"));

            //  Validation by property
            testSubject.DepentdentProperty = 3;
            testSubject.TestProperty = 2;
            Assert.AreNotEqual(string.Empty, testSubject.Validate("TestProperty"));
            testSubject.TestProperty = 3;
            Assert.AreEqual(string.Empty, testSubject.Validate("TestProperty"));
            testSubject.TestProperty = 5;
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


            public int DepentdentProperty { get; set; }

            [GreaterThanOrEqualTo(5, ErrorMessage = "Error1")]
            public int TestValue { get; set; }

            [GreaterThanOrEqualTo("DepentdentProperty", ValidationType.ByProperty, ErrorMessage = "Error2")]
            public int TestProperty { get; set; }
        }
    }
}
