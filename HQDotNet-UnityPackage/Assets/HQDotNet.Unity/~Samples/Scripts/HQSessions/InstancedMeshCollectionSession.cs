using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HQDotNet.Unity.Model;

namespace HQDotNet.Unity {
    public class InstancedMeshCollectionSession : MonoBehaviour {
        public MeshViewModel meshViewModel;
        public MeshDemoSettings sphereDemoSettingsModel;

        protected HQSession _session;

        public virtual void Awake() {
            _session = new HQSession();
            _session.RegisterController<SyncedSperesController>();
            _session.RegisterService<TransformCollectionService>();
            _session.RegisterView<DrawMeshInstancedCollectionView>();

            DispatchViewUpdate();
            DispatchDemoModelUpdate();
        }
        public virtual void Update() {
            _session.Update();
        }

        public virtual void LateUpdate() {
            _session.LateUpdate();
        }

        public virtual void OnDestroy() {
            _session.Shutdown();
        }

        protected void DispatchViewUpdate() {

            System.Action dispatchMessage(IModelListener<MeshViewModel> modelListener) {
                return () => modelListener.OnModelUpdated(ref meshViewModel);
            }

            _session.Dispatcher.Dispatch((HQDispatcher.DispatchMessageDelegate<IModelListener<MeshViewModel>>)dispatchMessage);

        }

        protected void DispatchDemoModelUpdate() {

            System.Action dispatchMessage(IModelListener<MeshDemoSettings> modelListener) {
                return () => modelListener.OnModelUpdated(ref sphereDemoSettingsModel);
            }

            _session.Dispatcher.Dispatch((HQDispatcher.DispatchMessageDelegate<IModelListener<MeshDemoSettings>>)dispatchMessage);

        }

    }
}
