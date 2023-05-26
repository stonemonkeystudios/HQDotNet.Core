using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity {
    public class HQViewMediator {
        public static HQViewMediator Instance { get; private set; }
        private HQSession _session;

        public static void CreateInstance(HQSession session) {
            Instance = new HQViewMediator(session);
        }

        public static void DestroyInstance() {
            Instance = null;
        }

        public HQViewMediator(HQSession session) {
            Instance = this;
            _session = session;
        }

        public void RegisterMonoView(HQMonoView monoView) {
            monoView.SetSession(_session);
        }
    }

}