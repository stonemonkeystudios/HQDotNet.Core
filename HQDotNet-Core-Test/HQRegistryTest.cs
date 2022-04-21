using System;
using System.Collections.Generic;
using HQDotNet.Model;
using NUnit.Framework;

namespace HQDotNet.Test {
    /*
     * Sublass dummy data
     */

    public class HQRegistryTest {

        HQRegistry _registry;

        [SetUp]
        public void Setup() {
            _registry = new HQRegistry();
        }

        [TearDown]
        public void Teardown() {
            _registry = null;
        }

        [Test]
        public void InstantiationTestSimple() {
            HQRegistry _registry = new HQRegistry();
            Assert.IsNotNull(_registry.Controllers);
            Assert.IsNotNull(_registry.Services);
            Assert.IsNotNull(_registry.Views);
            Assert.AreEqual(0, _registry.Controllers.Count);
            Assert.AreEqual(0, _registry.Services.Count);
            Assert.AreEqual(0, _registry.Views.Count);
        }

        #region General register/unregister helpers

        public void TestRegisterControllers(HQController[] controllers){

            for(int i = 0; i < controllers.Length; i++) {
                var controller = controllers[i];

                _registry.RegisterController(controller);
                Assert.AreEqual(i + 1, _registry.Controllers.Count);

                Assert.IsTrue(_registry.Controllers.ContainsKey(controller.GetType()));
                Assert.AreEqual(controller, _registry.Controllers[controller.GetType()]);
            }
        }

        public void TestRegisterServices(HQService[] services) {

            for (int i = 0; i < services.Length; i++) {
                var service = services[i];

                _registry.RegisterService(service);
                Assert.AreEqual(i + 1, _registry.Services.Count);

                Assert.IsTrue(_registry.Services.ContainsKey(service.GetType()));
                Assert.AreEqual(service, _registry.Services[service.GetType()]);
            }
        }

        public void TestRegisterViews(HQView[] views) {

            for (int i = 0; i < views.Length; i++) {
                var view = views[i];

                _registry.RegisterView(view);
                Assert.AreEqual(i + 1, _registry.Views.Count);

                Assert.IsTrue(_registry.Views.ContainsKey(view.GetType()));
                Assert.NotNull(_registry.Views[view.GetType()]);

                //Check the list of views for our view
                Assert.IsTrue(_registry.Views[view.GetType()].Contains(view));
                Assert.AreEqual(view, _registry.Views[view.GetType()][0]);
            }
        }

        public void TestUnregisterControllers(HQController[] controllers) {
            int startCount = _registry.Controllers.Count;

            for (int i = 0; i < controllers.Length; i++) {
                var controller = controllers[i];

                _registry.Unregister(controller);
                Assert.AreEqual(startCount - i - 1, _registry.Controllers.Count);

                Assert.IsFalse(_registry.Controllers.ContainsKey(controller.GetType()));
            }
        }

        public void TestUnregisterServices(HQService[] services) {
            int startCount = _registry.Services.Count;

            for (int i = 0; i < services.Length; i++) {
                var service = services[i];

                _registry.Unregister(service);
                Assert.AreEqual(startCount - i - 1, _registry.Services.Count);

                Assert.IsFalse(_registry.Services.ContainsKey(service.GetType()));
            }

        }

        public void TestUnregisterViews(HQView[] views) {
            int startCount = _registry.Views.Count;

            for (int i = 0; i < views.Length; i++) {
                var view = views[i];

                _registry.Unregister(view);
                Assert.AreEqual(startCount - i - 1, _registry.Views.Count);

                Assert.IsFalse(_registry.Views.ContainsKey(view.GetType()));
            }
        }

        #endregion

        #region Valid Registration

        [Test]
        public void TestRegisterBaseController() {
            var controller = new HQController();
            TestRegisterControllers(new HQController[] { controller });
        }

        [Test]
        public void TestRegisterDummyController() {
            var controller = new DummyModuleController();
            TestRegisterControllers(new HQController[] { controller });
        }

        [Test]
        public void TestRegisterMultipleControllers() {
            var _registry = new HQRegistry();
            var controller1 = new HQController();
            var controller2 = new DummyModuleController();
            TestRegisterControllers(new HQController[] { controller1, controller2 });
        }


        [Test]
        public void TestRegisterBaseService() {
            var service = new HQService();
            TestRegisterServices(new HQService[] { service });
        }

        [Test]
        public void TestRegisterDummyService() {
            var service = new DummyModuleService();
            TestRegisterServices(new HQService[] { service });
        }
        
        [Test]
        public void TestRegisterMultipleServices() {
            var _registry = new HQRegistry();
            var service1 = new HQService();
            var service2 = new DummyModuleService();

            TestRegisterServices(new HQService[] { service1, service2 });
        }

        [Test]
        public void TestRegisterBaseView() {
            var view = new HQView();
            TestRegisterViews(new HQView[] { view });
        }

        [Test]
        public void TestRegisterDummyView() {
            var view = new DummyModuleView();
            TestRegisterViews(new HQView[] { view });
        }

        [Test]
        public void TestRegisterMultipleViews() {
            var _registry = new HQRegistry();
            var view1 = new HQView();
            var view2 = new DummyModuleView();
            TestRegisterViews(new HQView[] { view1, view2 });
        }

        #endregion

        #region Valid Unregistration

        [Test]
        public void TestUnregisterBaseController() {
            HQController[] controllers = new HQController[] { new HQController() };
            TestRegisterControllers(controllers);
            TestUnregisterControllers(controllers);
        }

        [Test]
        public void TestUnregisterDummyController() {
            HQController[] controllers = new HQController[] { new DummyModuleController() };
            TestRegisterControllers(controllers);
            TestUnregisterControllers(controllers);
        }

        [Test]
        public void TestUnregisterMultipleControllers() {
            HQController[] controllers = new HQController[] { new HQController(), new DummyModuleController() };
            TestRegisterControllers(controllers);
            TestUnregisterControllers(controllers);
        }

        [Test]
        public void TestUnregisterBaseServices() {
            HQService[] services = new HQService[] { new HQService() };
            TestRegisterServices(services);
            TestUnregisterServices(services);
        }

        [Test]
        public void TestUnregisterDummyServices() {
            HQService[] services = new HQService[] { new DummyModuleService() };
            TestRegisterServices(services);
            TestUnregisterServices(services);
        }

        [Test]
        public void TestUnregisterMultipleServices() {
            HQService[] services = new HQService[] { new HQService(), new DummyModuleService() };
            TestRegisterServices(services);
            TestUnregisterServices(services);
        }

        [Test]
        public void TestUnregisterBaseViews() {
            HQView[] views = new HQView[] { new HQView() };
            TestRegisterViews(views);
            TestUnregisterViews(views);
        }

        [Test]
        public void TestUnregisterDummyViews() {
            HQView[] views = new HQView[] { new DummyModuleView() };
            TestRegisterViews(views);
            TestUnregisterViews(views);
        }

        [Test]
        public void TestUnregisterMultipleViews() {
            HQView[] views = new HQView[] { new HQView(), new DummyModuleView() };
            TestRegisterViews(views);
            TestUnregisterViews(views);
        }


        #endregion


        #region ListenerBinding Tests

        [Test]
        public void BindListenerTest() {
            HQView view = new HQView();
            _registry.BindListener(typeof(IModelListener<DummyData>), view);
            var listeners = _registry.GetDispatchListenersForType<IModelListener<DummyData>>();
            Assert.AreEqual(1, listeners.Count);
            Assert.IsTrue(listeners.Contains(view));
        }

        [Test]
        public void UnbindListenerTest() {
            HQView view = new HQView();
            _registry.BindListener(typeof(IModelListener<DummyData>), view);
            var listeners = _registry.GetDispatchListenersForType<IModelListener<DummyData>>();
            _registry.UnbindBehaviorListenerForObject(typeof(IModelListener<DummyData>), view);
            listeners = _registry.GetDispatchListenersForType<IModelListener<DummyData>>();
            Assert.AreEqual(0, listeners.Count);

        }

        [Test]
        public void UnbindListenerBehaviorTest() {
            HQView[] views = new HQView[] { new HQView(), new HQView() };
            _registry.BindListener(typeof(IModelListener<DummyData>), views[0]);
            _registry.BindListener(typeof(IModelListener<DummyData>), views[1]);
            var listeners = _registry.GetDispatchListenersForType<IModelListener<DummyData>>();
            Assert.AreEqual(2, listeners.Count);
            _registry.UnbindAllDispatchListenersForType(typeof(IModelListener<DummyData>));
            listeners = _registry.GetDispatchListenersForType<IModelListener<DummyData>>();
            Assert.AreEqual(0, listeners.Count);

        }

        #endregion

        #region Invalid Tests

        public void TestDuplicateControllers() {

        }

        public void TestDuplcateServices() {

        }

        public void TestDuplcateViews() {

        }

        /*
         * Unregister a behavior that isn't registered
         * Unregister a listener behavior that isn't registered
         * Unregister a listener type that isn't registered
         */

        #endregion
    }
}