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

        private HQRegistry _registry;
        private HQDispatcher _dispatcher;
        private HQInjector _injector;

        [SetUp]
        public void Setup() {
            _registry = new HQRegistry();
            _injector = new HQInjector();
            _dispatcher = new HQDispatcher();
            _injector.SetRegistry(_registry);
            _dispatcher.SetRegistry(_registry);
        }

        [TearDown]
        public void Teardown() {
            _dispatcher = null;
        }

        [Test]
        public void InstantiationTestSimple() {
            Assert.NotNull(_dispatcher);
        }

        [Test]
        public async Task SimpleListenerTest() {
            DummyModuleView view = new DummyModuleView();
            DummyModuleController controller = new DummyModuleController();
            DummyModuleService service = new DummyModuleService();

            _registry.RegisterController(_dispatcher);

            _registry.RegisterController(controller);
            _registry.RegisterView(view);
            _registry.RegisterService(service);

            _injector.Inject(controller);
            _injector.Inject(view);
            _injector.Inject(service);

            _dispatcher.RegisterListeners(controller);
            _dispatcher.RegisterListeners(view);
            _dispatcher.RegisterListeners(service);

            Assert.IsNull(view.DisplayString);
            Assert.True(controller.HasService());

            string dummyTitleString = "DummyTitle";

            await controller.QueryDummyDelayedServiceForData(dummyTitleString);

            _dispatcher.LateUpdate();

            Assert.AreEqual(dummyTitleString, view.DisplayString);
        }
    }
}