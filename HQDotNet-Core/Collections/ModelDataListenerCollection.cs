using System;

namespace HQDotNet.Model {
    public class ModelDataListenerCollection<TModelData> : DispatchListenerCollection<IModelListener<TModelData>>, IModelListener<TModelData>
        where TModelData : HQDataModel, new(){
        /*public void OnDataAdded(Type modelType, int id) {
            this.ForEach((listener) => listener.OnDataAdded(modelType, id));
        }

        public void OnDataDeleted(Type modelType, int id) {
            this.ForEach((listener) => listener.OnDataDeleted(modelType, id));
        }

        public void OnDataUpdated(Type modelType, int id) {
            this.ForEach((listener) => listener.OnDataUpdated(modelType, id));
        }*/
        /*
        void IModelListener.OnDataAdded(Type modelType, int id) {
            this.ForEach((listener) => listener.OnDataAdded(modelType, id));
        }

        void IModelListener.OnDataDeleted(Type modelType, int id) {
            this.ForEach((listener) => listener.OnDataDeleted(modelType, id));
        }

        void IModelListener.OnDataUpdated(Type modelType, int id) {
            this.ForEach((listener) => listener.OnDataUpdated(modelType, id));
        }*/

        void IModelListener<TModelData>.OnModelUpdated(TModelData model) {
            throw new NotImplementedException();
        }
    }
}
