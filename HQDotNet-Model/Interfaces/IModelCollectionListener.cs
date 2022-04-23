using System.Collections.Generic;

namespace HQDotNet {
    public interface IModelCollectionListener<TCollection, TModel> : IDispatchListener
        where TCollection : ICollection<TModel>{

        void OnModelAdded(TModel model);

        void OnModelUpdated(TModel model);

        void OnModelDeleted(TModel model);
    }
}
