using UnityEngine;

namespace HQDotNet.Unity {
    public class HQSessionSetupMonoBehavior : MonoBehaviour {
        protected HQSession _session;

        public virtual void Awake() {
            _session = new HQSession();
            HQViewMediator.CreateInstance(_session);
            MainThreadSyncer.CreateInstance();
            var views = FindObjectsOfType<HQMonoView>(true);
            foreach(var view in views) {
                view.SetSession(_session);
            }
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
            if(HQViewMediator.Instance != null)
                HQViewMediator.DestroyInstance();
            if (_session != null)
                _session.Shutdown();
            _session = null;
        }

        public virtual void OnApplicationQuit() {
        }
    }
}