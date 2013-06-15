using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace BigEgg.Framework.Applications.Collections
{
    /// <summary>
    /// Represents a collection of keys and values. And will raises the <see cref="E:PropertyChanged"/> event when a property value changes.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [Serializable]
    public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class.
        /// </summary>
        public ObservableDictionary()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public ObservableDictionary(IEqualityComparer<TKey> comparer)
            : base(comparer)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
        public ObservableDictionary(int capacity)
            : base(capacity)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="comparer">The comparer.</param>
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : base(dictionary, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="comparer">The comparer.</param>
        public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity, comparer)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
        protected ObservableDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {}
        #endregion

        #region Implement INotifyCollectionChanged interface
        protected virtual event NotifyCollectionChangedEventHandler collectionChanged;

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { CollectionChanged += value; }
            remove { CollectionChanged -= value; }
        }
        #endregion

        #region Implement INotifyPropertyChanged interface
        protected virtual event PropertyChangedEventHandler propertyChanged;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { this.propertyChanged += value; }
            remove { this.propertyChanged -= value; }
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The property name of the property that has changed.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            if (BEConfiguration.Debug) { CheckPropertyName(propertyName); }
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.propertyChanged != null) { this.propertyChanged(this, e); }
        }

        private void CheckPropertyName(string propertyName)
        {
            PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this)[propertyName];
            if (propertyDescriptor == null)
            {
                throw new InvalidOperationException(string.Format(null,
                    "The property with the propertyName '{0}' doesn't exist.", propertyName));
            }
        }
        #endregion

        #region Properties
        public override TValue this[TKey key]
        {
            get { return base[key]; }
            set
            {
                base[key] = value;
                RaisePropertyChanged(base.);
            }
        }
        #endregion
    }
}
