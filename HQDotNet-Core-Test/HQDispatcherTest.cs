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
    }
}