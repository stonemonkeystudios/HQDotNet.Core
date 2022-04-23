using System.Collections.Generic;

namespace HQDotNet {
    public interface IModelCollectionListener<TCollection, TModel> : IDispatchListener
        where TCollection : ICollection<TModel>{

        void OnModelAdded(ref TModel model);

        void OnModelUpdated(ref TModel model);

        void OnModelDeleted(ref TModel model);
    }
}
