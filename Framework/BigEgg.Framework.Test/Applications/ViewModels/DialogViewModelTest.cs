using BigEgg.Framework.Applications.ViewModels;
using BigEgg.Framework.Applications.Views;
using BigEgg.Framework.UnitTesting.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BigEgg.Framework.Test.Applications.ViewModels
{
    [TestClass]
    public class DialogViewModelTest
    {
        [TestMethod]
        public void DialogViewModelCloseTest()
        {
            MockDialogView view = new MockDialogView();
            MockDialogViewModel viewModel = new MockDialogViewModel(view);

            Assert.AreEqual("Mock Dialog", viewModel.Title);

            object owner = new object();
            Assert.IsFalse(view.IsVisible);
            view.ShowDialogAction = v =>
            {
                Assert.AreEqual(owner, v.Owner);
                Assert.IsTrue(v.IsVisible);
            };
            bool? dialogResult = viewModel.ShowDialog(owner);
            Assert.IsNull(dialogResult);
            Assert.IsFalse(view.IsVisible);
        }


        private class MockDialogViewModel : DialogViewModel<MockDialogView>
        {
            public MockDialogViewModel(MockDialogView view)
                : base(view)
            {
            }

            public override string Title
            {
                get { return "Mock Dialog"; }
            }
        }

        private class MockDialogView : MockDialogViewBase
        {
            public Action<MockDialogView> ShowDialogAction { get; set; }
            public MockDialogViewModel ViewModel { get { return ViewHelper.GetViewModel<MockDialogViewModel>(this); } }


            protected override void OnShowDialogAction()
            {
                if (ShowDialogAction != null) { ShowDialogAction(this); }
            }
        }
    }
}
