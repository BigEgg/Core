using BigEgg.Framework.Applications.Applications.Views;

namespace BigEgg.Framework.Applications.UnitTesting.Views
{
    /// <summary>
    /// The mock class for <see cref="IView"/> interface
    /// </summary>
    public class MockView : IView
    {
        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        /// <value>
        /// The data context.
        /// </value>
        public object DataContext { get; set; }
    }
}
