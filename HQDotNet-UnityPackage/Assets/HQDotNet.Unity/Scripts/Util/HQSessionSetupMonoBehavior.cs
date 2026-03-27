using UnityEngine;

namespace HQDotNet.Unity {
    public class HQSessionSetupMonoBehavior : MonoBehaviour {
        protected HQSession _session;
        protected HQContextRootMonoBehaviour _contextRoot;

        public virtual void Awake() {
            _session = new HQSession();
            _contextRoot = GetComponent<HQContextRootMonoBehaviour>();
            if (_contextRoot == null) {
                _contextRoot = gameObject.AddComponent<HQContextRootMonoBehaviour>();
            }

            _contextRoot.Initialize(_session);
            MainThreadSyncer.CreateInstance();
        }

        public virtual void Start() {

            if (_session != null)
                _session.Startup();
        }

        public virtual void Update() {
            if (_session != null)
                _session.Update();
        }

        public virtual void LateUpdate() {
            if (_session != null)
                _session.LateUpdate();
        }

        public virtual void OnDestroy() {
            if(MainThreadSyncer.Instance != null)
                MainThreadSyncer.DestroyInstance();

            if (_contextRoot != null) {
                _contextRoot.Shutdown();
            }

            if (_session != null)
                _session.Shutdown();

            _session = null;
            _contextRoot = null;
        }

        public virtual void OnApplicationQuit() {
        }
    }
}
