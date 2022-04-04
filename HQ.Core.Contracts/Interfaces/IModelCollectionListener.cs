using HQ.Model;

namespace HQ {
    public interface IModelCollectionListener<TModelType> : IDispatchListener where TModelType : HQModel {

        void OnModelAdded(TModelType model);

        void OnDataUpdated(TModelType model);
    }
}
