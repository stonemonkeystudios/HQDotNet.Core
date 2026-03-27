using System;
using UnityEngine;

namespace HQDotNet.Unity {
    [Obsolete("HQViewMediator is deprecated. Use HQContextRootMonoBehaviour instead.")]
    public class HQViewMediator {
        public static HQViewMediator Instance { get; private set; }
        private HQSession _session;

        [Obsolete("HQViewMediator is deprecated. Use HQContextRootMonoBehaviour instead.")]
        public static void CreateInstance(HQSession session) {
            Instance = new HQViewMediator(session);
            HQContextRootMonoBehaviour.SetLegacySession(session);
        }

        [Obsolete("HQViewMediator is deprecated. Use HQContextRootMonoBehaviour instead.")]
        public static void DestroyInstance() {
            HQContextRootMonoBehaviour.ClearLegacySession();
            Instance = null;
        }

        [Obsolete("HQViewMediator is deprecated. Use HQContextRootMonoBehaviour instead.")]
        public HQViewMediator(HQSession session) {
            Instance = this;
            _session = session;
            HQContextRootMonoBehaviour.SetLegacySession(session);
        }

        [Obsolete("HQViewMediator is deprecated. Use HQContextRootMonoBehaviour instead.")]
        public void RegisterMonoView(HQMonoView monoView) {
            monoView.SetSession(_session);
        }
    }

}
