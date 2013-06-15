using BigEgg.Framework.Foundation;
using BigEgg.Framework.Foundation.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;

namespace BigEgg.Framework.Test.Foundation.Validations
{
    [TestClass]
    public class IsPathRootedAttributeTest
    {
        [TestMethod]
        public void ValidationTest()
        {
            SomeClass testSubject = new SomeClass();

            Assert.AreEqual(string.Empty, testSubject.Validate("PathCanEmpty"));
            Assert.AreNotEqual(string.Empty, testSubject.Validate("PathCannotEmpty"));
            Assert.AreNotEqual(string.Empty, testSubject.Validate());

            //  Absolute Path
            testSubject.PathCanEmpty = @"c:\Windows";
            testSubject.PathCannotEmpty = @"c:\Windows";
            Assert.AreEqual(string.Empty, testSubject.Validate("PathCanEmpty"));
            Assert.AreEqual(string.Empty, testSubject.Validate("PathCannotEmpty"));
            Assert.AreEqual(string.Empty, testSubject.Validate());

            //  Unc Path
            testSubject.PathCanEmpty = @"\\myPc\mydir\myfile";
            testSubject.PathCannotEmpty = @"\\myPc\mydir\myfile";

            //  Relative Path
            testSubject.PathCanEmpty = @"\..\Windows";
            testSubject.PathCannotEmpty = @"\..\Windows";
            Assert.AreEqual(string.Empty, testSubject.Validate("PathCanEmpty"));
            Assert.AreEqual(string.Empty, testSubject.Validate("PathCannotEmpty"));
            Assert.AreEqual(string.Empty, testSubject.Validate());

            //  Not Path
            testSubject.PathCanEmpty = @"..\Windows";
            testSubject.PathCannotEmpty = @"..\Windows";
            Assert.AreNotEqual(string.Empty, testSubject.Validate("PathCanEmpty"));
            Assert.AreNotEqual(string.Empty, testSubject.Validate("PathCannotEmpty"));
            Assert.AreNotEqual(string.Empty, testSubject.Validate());

            //  Contains invalid characters 
            testSubject.PathCanEmpty = @"\<Windows>";
            testSubject.PathCannotEmpty = @"\<Windows>";
            Assert.AreNotEqual(string.Empty, testSubject.Validate("PathCanEmpty"));
            Assert.AreNotEqual(string.Empty, testSubject.Validate("PathCannotEmpty"));
            Assert.AreNotEqual(string.Empty, testSubject.Validate());
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


            [IsPathRooted(ErrorMessage = "Error1")]
            public string PathCanEmpty { get; set; }

            [IsPathRooted(false, ErrorMessage = "Error2")]
            public string PathCannotEmpty { get; set; }
        }
    }
}
