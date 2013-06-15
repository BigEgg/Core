using BigEgg.Framework.Applications.Views;
using System.Windows;
using System.Windows.Controls; 

namespace BigEgg.Presentation.Dialogs
{
    public abstract class DialogWindow : Window, IDialogView
    {
        public DialogWindow()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ShowInTaskbar = false;
            this.WindowStyle = System.Windows.WindowStyle.None;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;

            this.Style = (Style)FindResource("DialogStyle");
        }

        public override void OnApplyTemplate()
        {
            Button closeBtn = this.Template.FindName("CloseButton", this) as Button;
            closeBtn.Click += delegate
            {
                this.Close();
            };

            Border tp = this.Template.FindName("HeadBar", this) as Border;
            tp.MouseLeftButtonDown += delegate
            {
                this.DragMove();
            };
        }

        public virtual void ShowDialog(object owner)
        {
            Owner = owner as Window;
            ShowDialog();
        }
    }
}
