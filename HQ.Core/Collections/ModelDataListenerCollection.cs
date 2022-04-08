using System;

namespace HQDotNet {
    public class ModelDataListenerCollection : DispatchListenerCollection<IModelListener>, IModelListener {
        /*public void OnDataAdded(Type modelType, int id) {
            this.ForEach((listener) => listener.OnDataAdded(modelType, id));
        }

        public void OnDataDeleted(Type modelType, int id) {
            this.ForEach((listener) => listener.OnDataDeleted(modelType, id));
        }

        public void OnDataUpdated(Type modelType, int id) {
            this.ForEach((listener) => listener.OnDataUpdated(modelType, id));
        }*/
        void IModelListener.OnDataAdded(Type modelType, int id) {
            this.ForEach((listener) => listener.OnDataAdded(modelType, id));
        }

        void IModelListener.OnDataDeleted(Type modelType, int id) {
            this.ForEach((listener) => listener.OnDataDeleted(modelType, id));
        }

        void IModelListener.OnDataUpdated(Type modelType, int id) {
            this.ForEach((listener) => listener.OnDataUpdated(modelType, id));
        }
    }
}
