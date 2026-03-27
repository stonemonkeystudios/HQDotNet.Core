using UnityEngine;

namespace HQDotNet.Unity {
    /// <summary>
    /// Provides the active HQSession context to HQMonoView instances and can bind discovered views.
    /// </summary>
    public class HQContextRootMonoBehaviour : MonoBehaviour {
        public static HQContextRootMonoBehaviour Active { get; private set; }

        private static HQSession _legacySession;

        private HQSession _session;

        public HQSession Session => _session;

        public static bool TryGetActive(out HQContextRootMonoBehaviour contextRoot) {
            contextRoot = Active;
            return contextRoot != null;
        }

        internal static void SetLegacySession(HQSession session) {
            _legacySession = session;
        }

        internal static void ClearLegacySession() {
            _legacySession = null;
        }

        internal static bool TryGetLegacySession(out HQSession session) {
            session = _legacySession;
            return session != null;
        }

        public virtual void Initialize(HQSession session, bool bindDiscoveredViews = true) {
            _session = session;
            Active = this;

            if (bindDiscoveredViews) {
                BindDiscoveredViews();
            }
        }

        public virtual void Shutdown() {
            if (Active == this) {
                Active = null;
            }

            _session = null;
        }

        public virtual bool RegisterMonoView(HQMonoView monoView) {
            if (_session == null || monoView == null) {
                return false;
            }

            monoView.SetSession(_session);
            return true;
        }

        public virtual void BindDiscoveredViews() {
            var views = FindObjectsOfType<HQMonoView>(true);
            foreach (var view in views) {
                RegisterMonoView(view);
            }
        }
    }
}
