using NUnit.Framework;
using UnityEngine;

namespace HQDotNet.Unity.Tests.Editor {
    public class HQViewBindingCompatibilityTests {
        private class TestableMonoView : HQMonoView {
            public HQSession BoundSession => _session;

            public void InvokeAwake() {
                base.Awake();
            }
        }

        private class TestableSessionSetup : HQSessionSetupMonoBehavior {
            public HQSession Session => _session;
            public HQContextRootMonoBehaviour ContextRoot => _contextRoot;
        }

        [Test]
        public void NewBindingMode_ActiveContextRootBindsViewDuringAwake() {
            var session = new HQSession();
            var contextObject = new GameObject("context-root");
            var contextRoot = contextObject.AddComponent<HQContextRootMonoBehaviour>();
            contextRoot.Initialize(session, false);

            var viewObject = new GameObject("view");
            var view = viewObject.AddComponent<TestableMonoView>();
            view.InvokeAwake();

            Assert.AreSame(session, view.BoundSession);

            contextRoot.Shutdown();
            Object.DestroyImmediate(viewObject);
            Object.DestroyImmediate(contextObject);
        }

        [Test]
        public void NewBindingMode_SessionSetupProvidesContextRootAndBindsDiscoveredViews() {
            var sessionSetupObject = new GameObject("session-setup");
            var sessionSetup = sessionSetupObject.AddComponent<TestableSessionSetup>();

            var viewObject = new GameObject("view");
            var view = viewObject.AddComponent<TestableMonoView>();

            sessionSetup.Awake();

            Assert.IsNotNull(sessionSetup.ContextRoot);
            Assert.IsNotNull(sessionSetup.Session);
            Assert.AreSame(sessionSetup.Session, view.BoundSession);

            sessionSetup.OnDestroy();
            Object.DestroyImmediate(viewObject);
            Object.DestroyImmediate(sessionSetupObject);
        }

        [Test]
        public void LegacyBindingMode_ViewMediatorStillBindsView() {
            var session = new HQSession();

#pragma warning disable CS0618
            HQViewMediator.CreateInstance(session);
#pragma warning restore CS0618

            var viewObject = new GameObject("legacy-view");
            var view = viewObject.AddComponent<TestableMonoView>();
            view.InvokeAwake();

            Assert.AreSame(session, view.BoundSession);

#pragma warning disable CS0618
            HQViewMediator.DestroyInstance();
#pragma warning restore CS0618
            Object.DestroyImmediate(viewObject);
        }
    }
}
