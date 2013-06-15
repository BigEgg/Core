using BigEgg.Framework.Applications.ViewModels;
using BigEgg.Framework.Applications.Views;
using BigEgg.Framework.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Windows.Threading;

namespace BigEgg.Framework.Test.Applications.Views
{
    [TestClass]
    public class ViewHelperTest
    {
        [TestMethod]
        public void GetViewModelTest()
        {
            MockView view = new MockView();
            MockViewModel viewModel = new MockViewModel(view);

            Assert.AreEqual(viewModel, view.GetViewModel<MockViewModel>());

            AssertHelper.ExpectedException<ArgumentNullException>(() => ViewHelper.GetViewModel(null));
        }

        [TestMethod]
        public void GetViewModelWithDispatcherTest()
        {
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());

            MockView view = new MockView();
            MockViewModel viewModel = new MockViewModel(view);

            Assert.AreEqual(viewModel, view.GetViewModel<MockViewModel>());
        }


        private class MockView : IView
        {
            public object DataContext { get; set; }
        }

        private class MockViewModel : ViewModel
        {
            public MockViewModel(IView view)
                : base(view)
            {
            }

            private static void SetDataContext(IView view, ViewModel viewModel)
            {
                view.DataContext = viewModel;
            }
        }
    }
}
