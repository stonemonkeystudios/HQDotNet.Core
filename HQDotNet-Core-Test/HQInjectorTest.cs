using System;
using System.Collections.Generic;
using HQDotNet.Model;
using NUnit.Framework;

namespace HQDotNet.Test {
    /*
     * Sublass dummy data
     */

    public class HQInjectorTest {

        private HQRegistry _registry;
        private HQInjector _injector;

        [SetUp]
        public void Setup() {
            _registry = new HQRegistry();
            _injector = new HQInjector();
            _injector.SetRegistry(_registry);
        }

        [TearDown]
        public void Teardown() {
            _injector = null;
        }

        [Test]
        public void InstantiationTestSimple() {
            Assert.NotNull(_injector);
        }

        public void ValidInjectionTest_OperationOrderSwap() {
            var controller1 = new DummyModuleController();
            var controller2 = new DummyModuleControllerInvalid();

            _registry.RegisterController(controller1);
            _injector.Inject(controller1);
            _registry.RegisterController(controller2);
            _injector.Inject(controller2);
        }

        [Test]
        public void ValidInjectionTest_ControllerController() {
            var controller1 = new DummyModuleController();
            var controller2 = new DummyModuleControllerInvalid();

            _registry.RegisterController(controller1);
            _registry.RegisterController(controller2);

            _injector.Inject(controller1);
            _injector.Inject(controller2);

            Assert.True(controller1.HasController2());
        }

        [Test]
        public void ValidInjectionTest_ControllerService() {
            var controller = new DummyModuleController();
            var service = new DummyModuleService();

            _registry.RegisterController(controller);
            _registry.RegisterService(service);

            _injector.Inject(controller);
            _injector.Inject(service);

            Assert.IsTrue(controller.HasService());
        }

        [Test]
        public void ValidInjectionTest_ControllerView() {
            var controller = new DummyModuleController();
            var view = new DummyModuleView();

            _registry.RegisterController(controller);
            _registry.RegisterView(view);

            _injector.Inject(controller);
            _injector.Inject(view);

            Assert.IsTrue(view.HasController());
        }


        public void InvalidInjectionTest() {
            var validController = new DummyModuleController();
            var invalidController = new DummyModuleControllerInvalid();

            var validView = new DummyModuleView();
            var invalidView = new DummyModuleViewInvalid();

            var validService = new DummyModuleService();
            var invalidService = new DummyModuleServiceInvalid();

            _registry.RegisterController(validController);
            _registry.RegisterController(invalidController);

            _registry.RegisterView(validView);
            _registry.RegisterView(invalidView);

            _registry.RegisterService(validService);
            _registry.RegisterService(invalidService);

            _injector.Inject(validService);
            _injector.Inject(invalidService);

            Assert.IsFalse(invalidService.HasController());
            Assert.IsFalse(invalidService.HasService());
            Assert.IsFalse(invalidService.HasView());

            Assert.IsFalse(invalidController.HasView());

            Assert.IsFalse(invalidView.HasView());
            Assert.IsFalse(invalidView.HasService());
        }
    }
}