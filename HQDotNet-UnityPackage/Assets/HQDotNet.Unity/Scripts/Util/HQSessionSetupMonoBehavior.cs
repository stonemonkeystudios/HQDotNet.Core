using UnityEngine;

namespace HQDotNet.Unity {
    public class HQSessionSetupMonoBehavior : MonoBehaviour {
        protected HQSession _session;
        protected HQViewMediator _mediator;

        public virtual void Awake() {
            _session = new HQSession();
            _mediator = new HQViewMediator(_session);
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

        public virtual void OnApplicationQuit() {
            if (_session != null)
                _session.Shutdown();
        }
    }
}