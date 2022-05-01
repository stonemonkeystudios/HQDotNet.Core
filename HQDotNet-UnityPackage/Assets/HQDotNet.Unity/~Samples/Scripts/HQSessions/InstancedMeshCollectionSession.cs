using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HQDotNet.Unity.Model;

namespace HQDotNet.Unity {
    public class InstancedMeshCollectionSession : MonoBehaviour {
        public MeshViewModel meshViewModel;
        public MeshDemoSettings sphereDemoSettingsModel;

        private HQSession _session;

        public void Awake() {
            _session = new HQSession();
            _session.RegisterController<SyncedSperesController>();
            _session.RegisterService<TransformCollectionService>();
            _session.RegisterView<DrawMeshInstancedCollectionView>();

            DispatchViewUpdate();
            DispatchDemoModelUpdate();
        }
        public void Update() {
            _session.Update();
        }

        public void LateUpdate() {
            _session.LateUpdate();
        }

        public void OnDestroy() {
            _session.Shutdown();
        }

        private void DispatchViewUpdate() {

            System.Action dispatchMessage(IModelListener<MeshViewModel> modelListener) {
                return () => modelListener.OnModelUpdated(ref meshViewModel);
            }

            _session.Dispatcher.Dispatch((HQDispatcher.DispatchMessageDelegate<IModelListener<MeshViewModel>>)dispatchMessage);

        }

        private void DispatchDemoModelUpdate() {

            System.Action dispatchMessage(IModelListener<MeshDemoSettings> modelListener) {
                return () => modelListener.OnModelUpdated(ref sphereDemoSettingsModel);
            }

            _session.Dispatcher.Dispatch((HQDispatcher.DispatchMessageDelegate<IModelListener<MeshDemoSettings>>)dispatchMessage);

        }

    }
}
