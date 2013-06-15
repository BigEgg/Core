using BigEgg.Framework.Applications.ViewModels;
using BigEgg.Framework.Applications.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigEgg.Framework.Test.Applications.ViewModels
{
    [TestClass]
    public class ViewModelTest
    {
        [TestMethod]
        public void GetViewTest()
        {
            IView view = new MockView();
            DummyViewModel viewModel = new DummyViewModel(view);

            Assert.AreEqual(view, viewModel.View);
        }



        private class DummyViewModel : ViewModel
        {
            public DummyViewModel(IView view)
                : base(view)
            {
            }
        }


        private class MockView : IView
        {
            public object DataContext { get; set; }
        }
    }
}
