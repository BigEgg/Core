using BigEgg.Framework.Applications.Applications.Views;
using System;

namespace BigEgg.Framework.Applications.UnitTesting.Views
{
    /// <summary>
    /// The mock class for <see cref="IDialogView"/> interface
    /// </summary>
    public abstract class MockDialogView : MockView, IDialogView
    {
        /// <summary>
        /// Gets a value indicating whether this instance is visible.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisible { get; private set; }
        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>
        /// The owner.
        /// </value>
        public object Owner { get; private set; }
        /// <summary>
        /// Gets or sets the show dialog action.
        /// </summary>
        /// <value>
        /// The show dialog action.
        /// </value>
        public Action<MockDialogView> ShowDialogAction { get; set; }

        /// <summary>
        /// Show the dialog view with an owner.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public void ShowDialog(object owner)
        {
            Owner = owner;
            IsVisible = true;
            OnShowDialogAction();
            IsVisible = false;
            Owner = null;
        }

        /// <summary>
        /// Close the dialog view.
        /// </summary>
        public void Close()
        {
            IsVisible = false;
            Owner = null;
        }

        /// <summary>
        /// Called when show the dialog view.
        /// </summary>
        protected void OnShowDialogAction()
        {
            if (ShowDialogAction != null) { ShowDialogAction(this); }
        }
    }
}
