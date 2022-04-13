using HQDotNet.Model;

namespace HQDotNet {
    public interface IModelCollectionListener<TModelType> : IDispatchListener where TModelType : HQDataModel {

        void OnModelAdded(TModelType model);

        void OnDataUpdated(TModelType model);
    }
}
