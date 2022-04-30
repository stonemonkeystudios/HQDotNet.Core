using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity {
    public class TransformCollectionService : HQService {
        public List<Matrix4x4> LocalCollection { get; private set; }

        public TransformCollectionService() {
            LocalCollection = new List<Matrix4x4>();
        }

        public void Add(Matrix4x4 matrix) {

            LocalCollection.Add(matrix);

            int lastIndex = LocalCollection.Count - 1;
            var indexedModel = new IndexedDataModel<Matrix4x4>(lastIndex, ref matrix);

            System.Action dispatchMessage(IModelCollectionListener<List<IndexedDataModel<Matrix4x4>>, IndexedDataModel< Matrix4x4>> collectionListener) {
                return () => collectionListener.OnModelAdded(ref indexedModel);
            }

            var dispatchDelegate = 
                (HQDispatcher.DispatchMessageDelegate
                    <IModelCollectionListener
                        <List<IndexedDataModel<Matrix4x4>>,
                        IndexedDataModel<Matrix4x4>>
                    >
                )dispatchMessage;

            Session.Dispatcher.Dispatch(dispatchDelegate);
        }

        public void Add() {
            var newMatrix = new Matrix4x4();
            Add(newMatrix);
        }

        public void Remove(ref IndexedDataModel<Matrix4x4> modelToRemove) {
            //TODO
            throw new System.NotImplementedException();
            /*
            if (modelToRemove.index < LocalCollection.Count)
                return;

            LocalCollection.RemoveAt(modelToRemove.index);

            System.Action dispatchMessage(IModelCollectionListener<List<IndexedDataModel<Matrix4x4>>, IndexedDataModel<Matrix4x4>> collectionListener) {
                return () => collectionListener.OnModelDeleted(ref modelToRemove);
            }

            Session.Dispatcher.Dispatch(
                (HQDispatcher.DispatchMessageDelegate<IModelCollectionListener<List<Matrix4x4>, Matrix4x4>>)dispatchMessage);
            }*/
        }


        /*Session.Dispatcher.Dispatch((IModelListener<DummyData> modelListener) => {
                return () => modelListener.OnModelUpdated(ref data);
            });
        
         
         
            System.Action dispatchMessage(ISessionListener sessionListener) {
                return () => sessionListener.PhaseUpdated(Phase);
            }

            _dispatcher.Dispatch((HQDispatcher.DispatchMessageDelegate<ISessionListener>)dispatchMessage);
         
         
         */

    }
}

