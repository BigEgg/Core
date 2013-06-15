using BigEgg.Framework.Foundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.DataAnnotations;

namespace BigEgg.Framework.Test.Foundation
{
    [TestClass]
    public class ValidationModelTest
    {
        [TestMethod]
        public void ValidationTest()
        {
            Person person = new Person();

            Assert.IsNull(person.Name);
            Assert.IsFalse(string.IsNullOrEmpty(person.Validate()));

            person.Name = "BigEgg";
            Assert.IsTrue(string.IsNullOrEmpty(person.Validate()));
        }


        [Serializable]
        private class Person : ValidationModel
        {
            private string name;

            [Required]
            public string Name
            {
                get { return this.name; }
                set
                {
                    if (this.name != value)
                    {
                        this.name = value;
                        RaisePropertyChanged("Name");
                    }
                }
            }
        }
    }
}
