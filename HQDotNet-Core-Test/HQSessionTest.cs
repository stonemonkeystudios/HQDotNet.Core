using HQDotNet.Model;
using NUnit.Framework;
using System.Threading.Tasks;

namespace HQDotNet.Test {

    public class HQSessionTest {

        private class DummyModuleControllerDerived : DummyModuleController {
        }

        private HQSession _session;

        [SetUp]
        public void Setup() {
            _session = new HQSession();
            _session.Startup();
        }

        [TearDown]
        public void Teardown() {
            _session.Shutdown();
            _session = null;
        }

        /// <summary>
        /// Demonstrates a basic example of the functionality of HQSession
        /// Shows the cooperative automation of the Registry, Injector, and Dispatcher
        /// </summary>
        [Test]
        public async Task SimpleSessionTest() {
            string dummyTitleString = "DummyTitle";
            var controller = _session.RegisterController<DummyModuleController>();
            var view = _session.RegisterView<DummyModuleView>();
            _session.RegisterService<DummyModuleService>();

            Assert.IsNull(view.DisplayString);

            await controller.QueryDummyDelayedServiceForData(dummyTitleString);

            _session.LateUpdate();

            Assert.AreEqual(dummyTitleString, view.DisplayString);
        }

        [Test]
        public async Task SessionTestLoopedServiceQuery() {
            string dummyTitleString = "DummyTitle";
            var controller = _session.RegisterController<DummyModuleController>();
            var view = _session.RegisterView<DummyModuleView>();
            _session.RegisterService<DummyModuleService>();

            for (int i = 0; i < 10000; i++) {
                await controller.QueryDummyImmediateServiceForData(dummyTitleString + "_" + i);
                _session.LateUpdate();
                Assert.AreEqual(dummyTitleString + "_" + i, view.DisplayString);
            }

        }

        [Test]
        public void RegisterControllerTest() {
            var controller1 = _session.RegisterController<DummyModuleController>();
            var controller2 = _session.RegisterController<DummyModuleController2>();
            Assert.True(controller1.HasController());
            Assert.True(controller2.HasController());
        }

        [Test]
        public void RegisterServiceTest() {
            var controller1 = _session.RegisterController<DummyModuleController>();
            _session.RegisterService<DummyModuleService>();
            Assert.True(controller1.HasService());
        }

        [Test]
        public void RegisterController_BeforeStartup_IsInitializedAndAvailable() {
            var session = new HQSession();

            var controller = session.RegisterController<DummyModuleController>();

            Assert.AreEqual(HQPhase.Initialized, controller.Phase);
            Assert.AreSame(controller, session.GetController<DummyModuleController>());

            session.Shutdown();
        }

        [Test]
        public void RegisterController_AfterStartup_IsInitializedAndAvailable() {
            var controller = _session.RegisterController<DummyModuleController>();

            Assert.AreEqual(HQPhase.Initialized, controller.Phase);
            Assert.AreSame(controller, _session.GetController<DummyModuleController>());
        }

        [Test]
        public void RegisterService_BeforeStartup_IsInitializedAndAvailable() {
            var session = new HQSession();

            var service = session.RegisterService<DummyModuleService>();

            Assert.AreEqual(HQPhase.Initialized, service.Phase);
            Assert.AreSame(service, session.GetService<DummyModuleService>());

            session.Shutdown();
        }

        [Test]
        public void RegisterService_AfterStartup_IsInitializedAndAvailable() {
            var service = _session.RegisterService<DummyModuleService>();

            Assert.AreEqual(HQPhase.Initialized, service.Phase);
            Assert.AreSame(service, _session.GetService<DummyModuleService>());
        }

        [Test]
        public void RegisterView_BeforeStartup_IsInitializedAndRegisteredAsDispatchListener() {
            var session = new HQSession();

            var view = session.RegisterView<DummyModuleView>();

            Assert.AreEqual(HQPhase.Initialized, view.Phase);
            Assert.Contains(view, session.Dispatcher.GetListeners<IModelListener<DummyData>>());

            session.Shutdown();
        }

        [Test]
        public void RegisterView_AfterStartup_IsInitializedAndRegisteredAsDispatchListener() {
            var view = _session.RegisterView<DummyModuleView>();

            Assert.AreEqual(HQPhase.Initialized, view.Phase);
            Assert.Contains(view, _session.Dispatcher.GetListeners<IModelListener<DummyData>>());
        }

        [Test]
        public void RegisterController_DuplicateRegistration_ReturnsNull() {
            var firstController = _session.RegisterController<DummyModuleController>();

            var secondController = _session.RegisterController<DummyModuleController>();

            Assert.NotNull(firstController);
            Assert.IsNull(secondController);
        }

        [Test]
        public void RegisterService_DuplicateRegistration_ReturnsNull() {
            var firstService = _session.RegisterService<DummyModuleService>();

            var secondService = _session.RegisterService<DummyModuleService>();

            Assert.NotNull(firstService);
            Assert.IsNull(secondService);
        }

        [Test]
        public void RegisterView_DuplicateTypeRegistration_ReturnsDistinctInstances() {
            var firstView = _session.RegisterView<DummyModuleView>();
            var secondView = _session.RegisterView<DummyModuleView>();

            Assert.NotNull(firstView);
            Assert.NotNull(secondView);
            Assert.AreNotSame(firstView, secondView);
            Assert.AreEqual(2, _session.Dispatcher.GetListeners<IModelListener<DummyData>>().Count);
        }

        [Test]
        public void GetService_ExactAndSupertypeRequests_ReturnRegisteredSubtype() {
            var registeredService = _session.RegisterService<DummyModuleServiceInherited>();

            var exactTypeLookup = _session.GetService<DummyModuleServiceInherited>();
            var supertypeLookup = _session.GetService<DummyModuleService>();

            Assert.AreSame(registeredService, exactTypeLookup);
            Assert.AreSame(registeredService, supertypeLookup);
        }

        [Test]
        public void GetService_SubtypeRequestAgainstRegisteredBase_ReturnsNull() {
            _session.RegisterService<DummyModuleService>();

            var subtypeLookup = _session.GetService<DummyModuleServiceInherited>();

            Assert.IsNull(subtypeLookup);
        }

        [Test]
        public void GetController_ExactRequest_ReturnsRegisteredInstance() {
            var registeredController = _session.RegisterController<DummyModuleController>();

            var lookup = _session.GetController<DummyModuleController>();

            Assert.AreSame(registeredController, lookup);
        }

        [Test]
        public void GetController_SubtypeAndSupertypeRequests_ReturnNull() {
            _session.RegisterController<DummyModuleController>();

            var subtypeLookup = _session.GetController<DummyModuleControllerDerived>();
            var supertypeLookup = _session.GetController<HQController>();

            Assert.IsNull(subtypeLookup);
            Assert.IsNull(supertypeLookup);
        }

        [Test]
        public void RegisterForDispatchOnlyTest() {
            string newText = "newText";
            DummyNonHQClass classInstance = new DummyNonHQClass();
            Assert.AreEqual("", classInstance.textToUpdate);
            _session.RegisterObjectOnlyForDispatch(classInstance);
            _session.Startup();
            _session.Dispatcher.Dispatch<IDummyListener>(listener => listener.UpdateText(newText));
            _session.Update();
            _session.LateUpdate();
            Assert.AreEqual(newText, classInstance.textToUpdate);
        }

        [Test]
        public void SubclassInjectionTest() {
            var controller = _session.RegisterController<DummyModuleController>();
            _session.RegisterService<DummyModuleServiceInherited>();
            Assert.IsTrue(controller.HasService());
        }

        [Test]
        public void Unregister_RemovesInjectedReferencesAndDispatchListeners() {
            var controller = _session.RegisterController<DummyModuleController>();
            var service = _session.RegisterService<DummyModuleService>();
            var view = _session.RegisterView<DummyModuleView>();

            Assert.IsTrue(controller.HasService());
            Assert.IsTrue(view.HasService());
            Assert.AreEqual(1, _session.Dispatcher.GetListeners<IModelListener<DummyData>>().Count);

            _session.Unregister(service);

            Assert.IsFalse(controller.HasService());
            Assert.IsFalse(view.HasService());
            Assert.IsNull(_session.GetService<DummyModuleService>());

            _session.Dispatcher.Dispatch<IModelListener<DummyData>>(listener => listener.OnModelUpdated(new DummyData() { title = "before unregister" }));
            Assert.AreEqual("before unregister", view.DisplayString);

            _session.Unregister(view);

            Assert.AreEqual(0, _session.Dispatcher.GetListeners<IModelListener<DummyData>>().Count);
            _session.Dispatcher.Dispatch<IModelListener<DummyData>>(listener => listener.OnModelUpdated(new DummyData() { title = "after unregister" }));
            Assert.AreEqual("before unregister", view.DisplayString);
        }

        [Test]
        public void UnregisterController_RemovesControllerAndUninjectsDependents() {
            var controller = _session.RegisterController<DummyModuleController>();
            var dependentController = _session.RegisterController<DummyModuleController2>();

            Assert.IsTrue(dependentController.HasController());

            _session.Unregister(controller);

            Assert.IsNull(_session.GetController<DummyModuleController>());
            Assert.IsFalse(dependentController.HasController());
        }

        [Test]
        public void RuntimeContextInterface_RegisterAndLookupController_Works() {
            IHQRuntimeContext runtimeContext = new HQSession();

            var controller = runtimeContext.RegisterController<DummyModuleController>();
            var lookup = runtimeContext.GetController<DummyModuleController>();

            Assert.NotNull(controller);
            Assert.AreSame(controller, lookup);

            (runtimeContext as HQSession).Shutdown();
        }

        [Test]
        public void RuntimeContextInterface_RegisterAndLookupService_Works() {
            IHQRuntimeContext runtimeContext = new HQSession();

            var service = runtimeContext.RegisterService<DummyModuleService>();
            var lookup = runtimeContext.GetService<DummyModuleService>();

            Assert.NotNull(service);
            Assert.AreSame(service, lookup);

            (runtimeContext as HQSession).Shutdown();
        }
    }
}
