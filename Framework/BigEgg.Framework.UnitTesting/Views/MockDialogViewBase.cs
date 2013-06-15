using BigEgg.Framework.Applications.Views;
using BigEgg.Framework.Presentation;

namespace BigEgg.Framework.UnitTesting.Views
{
    public abstract class MockDialogViewBase : MockView, IDialogView
    {
        public bool IsVisible { get; private set; }
        public object Owner { get; private set; }


        public void ShowDialog(object owner)
        {
            Owner = owner;
            IsVisible = true;
            OnShowDialogAction();
            IsVisible = false;
            Owner = null;
        }

        public void Close()
        {
            IsVisible = false;
            Owner = null;
        }


        protected abstract void OnShowDialogAction();
    }
}
