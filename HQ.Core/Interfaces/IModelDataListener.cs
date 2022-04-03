using System;

namespace HQ {
    /// <summary>
    /// An interface for an object that would like to receive updates about a data store.
    /// If using [Inject] on a private Model field, this object will be automatically registered for notification.
    /// </summary>
    ///
    public interface IModelDataListener : IDispatchListener {
        public delegate void DataAddedDelegate(Type modelType, int id);

        /// <summary>
        /// Data has been added to a Model
        /// </summary>
        /// <param name="modelType">Type of Model that's been updated.</param>
        /// <param name="id">Id of the updated data.</param>
        void OnDataAdded(Type modelType, int id);

        /// <summary>
        /// Data has been deleted from a Model
        /// </summary>
        /// <param name="modelType">Type of Model that's been deleted from.</param>
        /// <param name="id">Id of the deleted data.</param>
        void OnDataDeleted(Type modelType, int id);

        /// <summary>
        /// Data has been updated in a Model
        /// </summary>
        /// <param name="modelType">Type of Model that's been updated.</param>
        /// <param name="id">Id of the updated data.</param>
        void OnDataUpdated(Type modelType, int id);
    }
}