using BigEgg.Framework.Foundation;
using BigEgg.Framework.Foundation.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;

namespace BigEgg.Framework.Test.Foundation.Validations
{
    [TestClass]
    public class RequiredIfAttributeTest
    {
        [TestMethod]
        public void ValidationTest()
        {
            SomeClass testSubject = new SomeClass();

            Assert.AreEqual(string.Empty, testSubject.Validate("TestValue1"));
            Assert.AreEqual(string.Empty, testSubject.Validate("TestValue2"));
            Assert.AreEqual(string.Empty, testSubject.Validate());

            testSubject.DepentdentProperty = true;
            Assert.AreNotEqual(string.Empty, testSubject.Validate("TestValue1"));
            testSubject.TestValue1 = "AAA";
            Assert.AreEqual(string.Empty, testSubject.Validate("TestValue1"));
            Assert.AreEqual(string.Empty, testSubject.Validate());

            testSubject.TestValue1 = "abc";
            Assert.AreEqual(string.Empty, testSubject.Validate("TestValue1"));
            Assert.AreNotEqual(string.Empty, testSubject.Validate());
            testSubject.TestValue2 = "BBB";
            Assert.AreEqual(string.Empty, testSubject.Validate());
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

                this.DepentdentProperty = false;
                this.DepentdentString = "abc";
            }


            public bool DepentdentProperty { get; set; }
            public string DepentdentString { get; set; }

            [RequiredIf("DepentdentProperty", true, ErrorMessage = "Error1")]
            public string TestValue1 { get; set; }

            [RequiredIf("TestValue1", "DepentdentString", validationType: ValidationType.ByProperty, ErrorMessage = "Error1")]
            public string TestValue2 { get; set; }
        }
    }
}
