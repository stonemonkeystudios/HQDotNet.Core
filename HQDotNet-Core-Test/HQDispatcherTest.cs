using System;
using System.Collections.Generic;
using HQDotNet.Model;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace HQDotNet.Test {
    /*
     * Sublass dummy data
     */

    public class HQDispatcherTest {
        private HQSession _session;

        [SetUp]
        public void Setup() {
            _session = new HQSession();
            _session.Startup();
        }

        [TearDown]
        public void Teardown() {
            _session = null;
        }

        [Test]
        public void InstantiationTestSimple() {
            Assert.NotNull(_session.Dispatcher);
        }

        [Test]
        public async Task DelayedServiceDispatchTest() {
            var view = _session.RegisterView<DummyModuleView>();
            var controller = _session.RegisterController<DummyModuleController>();
            _session.RegisterService<DummyModuleService>();

            Assert.IsNull(view.DisplayString);
            Assert.True(controller.HasService());

            //Does nothing, but simulating flow of control
            _session.Update();

            string dummyTitleString = "DummyTitle";

            await controller.QueryDummyDelayedServiceForData(dummyTitleString);

            //Actually dispatches a message queued in the call above.
            //We are awaiting it here, but in practice, a real dispatch might be queued in a few frames
            _session.LateUpdate();

            Assert.AreEqual(dummyTitleString, view.DisplayString);
        }

        [Test]
        public async Task ImmediateServiceDispatchTest() {
            var view = _session.RegisterView<DummyModuleView>();
            var controller = _session.RegisterController<DummyModuleController>();
            _session.RegisterService<DummyModuleService>();

            Assert.IsNull(view.DisplayString);
            Assert.True(controller.HasService());

            //Does nothing, but simulating flow of control
            _session.Update();

            string dummyTitleString = "DummyTitle";

            await controller.QueryDummyImmediateServiceForData(dummyTitleString);

            //Actually dispatch occurs on main thread in LateUpdate
            _session.Dispatcher.LateUpdate();

            Assert.AreEqual(dummyTitleString, view.DisplayString);
        }


        #region Non-HQ Tests

        public interface INonHQDispatchTest : IDispatchListener {
            void DispatchAction(int newCount);
        }
        public interface INonHQDispatchTestString : IDispatchListener {
            void DispatchAction(string newCount);
        }
        public class NonHQDispatchTest : NonHQDispatchTestBaseClass, INonHQDispatchTest, INonHQDispatchTestString {
            public int count = 0;
            public string name = "";
            public void DispatchAction(int newCount) {
                count = newCount;
            }

            public void DispatchAction(string newString) {
                name = newString;
            }
        }

        public class NonHQDispatchTestBaseClass { 
        }

        public class NonHQDispatchTestHQSideObject : HQView, INonHQDispatchTest, INonHQDispatchTestString {
            public int count = 0;
            public string name = "";
            public void DispatchAction(int newCount) {
                count = newCount;
            }

            public void DispatchAction(string newString) {
                name = newString;
            }
        }

        [Test]
        public void TestNonHQRegistration() {
            Setup();

            var hqView = _session.RegisterView<NonHQDispatchTestHQSideObject>();

            var nonHQView = new NonHQDispatchTest();
            _session.RegisterObjectOnlyForDispatch(nonHQView);

            var listeners = _session.Dispatcher.GetListeners<INonHQDispatchTest>();
            Assert.IsNotNull(listeners);
            Assert.AreEqual(2, listeners.Count);

            //TODO: Actually dispatch here
            int newInt = 1;
            string newString = "Name";

            DispatchNonHQRegistrationTest(newInt);
            DispatchNonHQRegistrationTestString(newString);
            _session.LateUpdate();
            Assert.AreEqual(newInt, nonHQView.count);
            Assert.AreEqual(newInt, hqView.count);
            Assert.AreEqual(newString, nonHQView.name);
            Assert.AreEqual(newString, hqView.name);
        }

        [Test]
        public void TestNonHQRegistration2() {
            Setup();

            var hqView = _session.RegisterView<NonHQDispatchTestHQSideObject>();

            var nonHQView = new NonHQDispatchTest();
            _session.RegisterObjectOnlyForDispatch(nonHQView);

            var listeners = _session.Dispatcher.GetListeners<INonHQDispatchTest>();
            Assert.IsNotNull(listeners);
            Assert.AreEqual(2, listeners.Count);

            //TODO: Actually dispatch here
            int newInt = 1;
            string newString = "Name";

            _session.Dispatcher.Dispatch<INonHQDispatchTest>((listener) => { listener.DispatchAction(newInt); });
            _session.Dispatcher.Dispatch<INonHQDispatchTestString>((listener) => { listener.DispatchAction(newString); });

            //DispatchNonHQRegistrationTest(newInt);
            //DispatchNonHQRegistrationTestString(newString);
            _session.LateUpdate();
            Assert.AreEqual(newInt, nonHQView.count);
            Assert.AreEqual(newInt, hqView.count);
            Assert.AreEqual(newString, nonHQView.name);
            Assert.AreEqual(newString, hqView.name);
        }

        protected void DispatchNonHQRegistrationTest(int newInt) {

            Action dispatchMessage(INonHQDispatchTest hubListener) {
                return () => hubListener.DispatchAction(newInt);
            }
            var dispatchDelegate = (HQDispatcher.DispatchMessageDelegate<INonHQDispatchTest>)dispatchMessage;

            _session.Dispatcher.Dispatch(dispatchDelegate);
        }

        protected void DispatchNonHQRegistrationTestString(string newString){


            Action dispatchMessage(INonHQDispatchTestString hubListener) {
                return () => hubListener.DispatchAction(newString);
            }
            var dispatchDelegate = (HQDispatcher.DispatchMessageDelegate<INonHQDispatchTestString>)dispatchMessage;

            _session.Dispatcher.Dispatch(dispatchDelegate);
        }

        #endregion
    }
}