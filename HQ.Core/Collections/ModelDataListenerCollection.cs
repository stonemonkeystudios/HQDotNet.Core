using System;

namespace HQ {
    public class ModelDataListenerCollection : DispatchListenerCollection<IModelDataListener>, IModelDataListener {
        /*public void OnDataAdded(Type modelType, int id) {
            this.ForEach((listener) => listener.OnDataAdded(modelType, id));
        }

        public void OnDataDeleted(Type modelType, int id) {
            this.ForEach((listener) => listener.OnDataDeleted(modelType, id));
        }

        public void OnDataUpdated(Type modelType, int id) {
            this.ForEach((listener) => listener.OnDataUpdated(modelType, id));
        }*/
        void IModelDataListener.OnDataAdded(Type modelType, int id) {
            this.ForEach((listener) => listener.OnDataAdded(modelType, id));
        }

        void IModelDataListener.OnDataDeleted(Type modelType, int id) {
            this.ForEach((listener) => listener.OnDataDeleted(modelType, id));
        }

        void IModelDataListener.OnDataUpdated(Type modelType, int id) {
            this.ForEach((listener) => listener.OnDataUpdated(modelType, id));
        }
    }
}
