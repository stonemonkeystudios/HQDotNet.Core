using UnityEngine;
using HQDotNet;

namespace HQDotNet.Unity
{
    public class HQMonoView : MonoBehaviour
    {
        protected HQSession _session;

        /// <summary>
        /// If this view has not been registered with HQ yet, register it via the active HQ context root.
        /// Falls back to the legacy HQViewMediator compatibility path when needed.
        /// </summary>
        protected virtual void Awake() {
            if (_session != null) {
                return;
            }

            if (HQContextRootMonoBehaviour.TryGetActive(out var contextRoot)) {
                contextRoot.RegisterMonoView(this);
                return;
            }

            if (HQContextRootMonoBehaviour.TryGetLegacySession(out var legacySession)) {
                SetSession(legacySession);
            }
        }

        /// <summary>
        /// Set the active session for this HQMonoView
        /// This also registers the view for dispatches from HQ
        /// </summary>
        /// <param name="session"></param>
        public virtual void SetSession(HQSession session) {
            _session = session;
            _session.RegisterObjectOnlyForDispatch(this);
        }

        /// <summary>
        /// Unregisters this view from the given session when it is destroyed.
        /// </summary>
        private void OnDestroy() {
            if (_session != null) {
                _session.UnregisterNonHQBehaviorDispatch(this);
            }
        }
    }
}
