using BigEgg.Framework.Applications.Views;

namespace BigEgg.Framework.Applications.ViewModels
{
    /// <summary>
    /// Abstract base class for a DialogViewModel implementation.
    /// </summary>
    public abstract class DialogViewModel<TView> : ViewModel<TView>
        where TView : IDialogView
    {
        /// <summary>
        /// The dialog result
        /// </summary>
        protected bool? dialogResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogViewModel{TView}"/> class and
        /// attaches itself as <c>DataContext</c> to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        public DialogViewModel(TView view)
            : base(view)
        {
        }


        /// <summary>
        /// Gets the dialog title.
        /// </summary>
        public abstract string Title { get; }


        /// <summary>
        /// Show the dialog with an owner.
        /// </summary>
        /// <param name="owner">The owner of the dialog.</param>
        /// <returns>Return the dialog result.</returns>
        public virtual bool? ShowDialog(object owner)
        {
            ViewCore.ShowDialog(owner);
            return this.dialogResult;
        }


        /// <summary>
        /// Close the dialog with a result.
        /// </summary>
        /// <param name="dialogResult">The dialog result.</param>
        protected virtual void Close(bool? dialogResult)
        {
            this.dialogResult = dialogResult;
            ViewCore.Close();
        }
    }
}
