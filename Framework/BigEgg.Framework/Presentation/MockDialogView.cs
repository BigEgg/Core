using BigEgg.Framework.Applications.Views;

namespace BigEgg.Framework.Presentation
{
    public abstract class MockDialogView : MockView, IDialogView
    {
        public void ShowDialog(object owner)
        {
        }

        public void Close()
        {
        }
    }
}
