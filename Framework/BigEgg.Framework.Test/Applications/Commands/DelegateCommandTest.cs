using BigEgg.Framework.Applications.Commands;
using BigEgg.Framework.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BigEgg.Framework.Test.Applications.Commands
{
    [TestClass]
    public class DelegateCommandTest
    {
        public TestContext TestContext { get; set; }


        [TestMethod]
        public void ConstructorTest()
        {
            DelegateCommand<int?> command = new DelegateCommand<int?>(param => { });
            Assert.IsNotNull(command);

            //  Construct with generic type is Non-Nullable
            AssertHelper.ExpectedException<InvalidCastException>(() => new DelegateCommand<int>(param => { }));
        }


        [TestMethod]
        public void ExecuteTest()
        {
            bool executed = false;
            DelegateCommand command = new DelegateCommand(() => executed = true);

            command.Execute();
            Assert.IsTrue(executed);
        }

        [TestMethod]
        public void ExecuteTest2()
        {
            bool executed = false;
            object commandParameter = null;
            DelegateCommand<object> command = new DelegateCommand<object>((object parameter) =>
            {
                executed = true;
                commandParameter = parameter;
            });

            object obj = new object();
            command.Execute(obj);
            Assert.IsTrue(executed);
            Assert.AreEqual(obj, commandParameter);
        }

        [TestMethod]
        public void ExecuteTest3()
        {
            bool executed = false;
            bool canExecute = true;
            DelegateCommand command = new DelegateCommand(() => executed = true, () => canExecute);

            command.Execute();
            Assert.IsTrue(executed);
        }

        [TestMethod]
        public void ExecuteTest4()
        {
            bool executed = false;
            bool canExecute = false;
            DelegateCommand command = new DelegateCommand(() => executed = true, () => canExecute);

            AssertHelper.ExpectedException<InvalidOperationException>(() => command.Execute());
            Assert.IsFalse(executed);
        }

        [TestMethod]
        public void RaiseCanExecuteChangedTest()
        {
            bool executed = false;
            bool canExecute = false;
            DelegateCommand command = new DelegateCommand(() => executed = false, () => canExecute);

            Assert.IsFalse(command.CanExecute());
            canExecute = true;
            Assert.IsTrue(command.CanExecute());

            AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged());

            Assert.IsFalse(executed);
        }

        [TestMethod]
        public void ConstructorParameterTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => new DelegateCommand((Action)null));

            AssertHelper.ExpectedException<ArgumentNullException>(() => new DelegateCommand<object>((Action<object>)null));
        }
    }
}
