using System;
using System.ComponentModel;

namespace BigEgg.Framework.Foundation
{
    /// <summary>
    /// Defines the base class for a model, provides an <see cref="IDataErrorInfo"/> implementation.
    /// </summary>
    [Serializable]
    public abstract class ValidationModel : Model, IDataErrorInfo
    {
        /// <summary>
        /// The instance of a <see cref="DataErrorInfoSupport"/>
        /// </summary>
        protected readonly DataErrorInfoSupport dataErrorInfoSupport;


        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationModel" /> class.
        /// </summary>
        public ValidationModel()
        {
            this.dataErrorInfoSupport = new DataErrorInfoSupport(this);
        }


        string IDataErrorInfo.Error { get { return this.dataErrorInfoSupport.Error; } }

        string IDataErrorInfo.this[string columnName] { get { return this.dataErrorInfoSupport[columnName]; } }
    }
}
