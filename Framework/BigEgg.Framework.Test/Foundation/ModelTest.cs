using BigEgg.Framework.Foundation;
using BigEgg.Framework.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;

namespace BigEgg.Framework.Test.Foundation
{
    [TestClass]
    public class ModelTest
    {
        private bool originalDebugSetting;


        public TestContext TestContext { get; set; }


        [TestInitialize]
        public void TestInitialize()
        {
            this.originalDebugSetting = BEConfiguration.Debug;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            BEConfiguration.Debug = this.originalDebugSetting;
        }

        [TestMethod]
        public void RaisePropertyChangedTest()
        {
            Person luke = new Person();

            BEConfiguration.Debug = true;
            AssertHelper.PropertyChangedEvent(luke, x => x.Name, () => luke.Name = "Luke");

            BEConfiguration.Debug = false;
            AssertHelper.PropertyChangedEvent(luke, x => x.Name, () => luke.Name = "Skywalker");
        }

        [TestMethod]
        public void AddAndRemoveEventHandler()
        {
            Person luke = new Person();
            bool eventRaised;

            PropertyChangedEventHandler eventHandler = (sender, e) =>
            {
                eventRaised = true;
            };

            eventRaised = false;
            luke.PropertyChanged += eventHandler;
            luke.Name = "Luke";
            Assert.IsTrue(eventRaised, "The property changed event needs to be raised");

            eventRaised = false;
            luke.PropertyChanged -= eventHandler;
            luke.Name = "Luke Skywalker";
            Assert.IsFalse(eventRaised, "The event handler must not be called because it was removed from the event.");
        }

        [TestMethod]
        public void WrongPropertyName()
        {
            WrongDocument document = new WrongDocument();

            BEConfiguration.Debug = true;
            AssertHelper.ExpectedException<InvalidOperationException>(() =>
                document.Header = "BigEgg Application Framework");

            BEConfiguration.Debug = false;
            document.Header = "BigEgg Application Framework";
        }

        [Serializable]
        private class Person : Model
        {
            private string name;

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

        private class WrongDocument : Model
        {
            private string header;

            public string Header
            {
                get { return this.header; }
                set
                {
                    if (this.header != value)
                    {
                        this.header = value;
                        RaisePropertyChanged("WrongPropertyName");
                    }
                }
            }
        }
    }
}
