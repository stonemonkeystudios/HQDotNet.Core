using System;
using System.Collections.Generic;
using HQDotNet.Model;
using NUnit.Framework;

namespace HQDotNet.Test {
    /*
     * Sublass dummy data
     */

    public class HQRegistryTest {



        [SetUp]
        public void Setup() {
        }

        [Test]
        public void InstantiationTestSimple() {
            HQRegistry registry = new HQRegistry();
            Assert.IsNotNull(registry.Controllers);
            Assert.IsNotNull(registry.Services);
            Assert.IsNotNull(registry.Views);
            Assert.AreEqual(0, registry.Controllers.Count);
            Assert.AreEqual(0, registry.Services.Count);
            Assert.AreEqual(0, registry.Views.Count);
        }

        public void TestRegisterController(HQController controller){
            HQRegistry registry = new HQRegistry();

            registry.RegisterController(controller);;
            Assert.AreEqual(1, registry.Controllers.Count);

            Assert.True(registry.Controllers.ContainsKey(controller.GetType()));
            Assert.AreEqual(controller, registry.Controllers[controller.GetType()]);
        }

        public void TestRegisterService(HQService service) {
            HQRegistry registry = new HQRegistry();

            registry.RegisterService(service);
            Assert.AreEqual(1, registry.Services.Count);

            Assert.True(registry.Services.ContainsKey(service.GetType()));
            Assert.AreEqual(service, registry.Services[service.GetType()]);
        }

        public void TestRegisterView(HQView view) {
            HQRegistry registry = new HQRegistry();

            Assert.IsNotNull(view);

            registry.RegisterView(view);
            Assert.AreEqual(1, registry.Views.Count);

            //Check if our viewModelType was registered properly.
            Assert.True(registry.Views.ContainsKey(view.GetType()));
            Assert.NotNull(registry.Views[view.GetType()]);

            //Check the list of views for our view
            Assert.True(registry.Views[view.GetType()].Contains(view));
            Assert.AreEqual(view, registry.Views[view.GetType()][0]);
        }

        [Test]
        public void TestRegisterBaseController() {
            var controller = new HQController();
            TestRegisterController(controller);
        }

        [Test]
        public void TestRegisterDummyController() {
            var controller = new DummyModuleController();
            TestRegisterController(controller);
        }
        [Test]
        public void TestRegisterBaseService() {
            var service = new HQService();
            TestRegisterService(service);
        }

        [Test]
        public void TestRegisterDummyService() {
            var service = new DummyModuleService();
            TestRegisterService(service);
        }

        [Test]
        public void TestRegisterBaseView() {
            var view = new HQView();
            TestRegisterView(view);
        }

        [Test]
        public void TestRegisterDummyView() {
            var view = new DummyModuleView();
            TestRegisterView(view);
        }

        [Test]
        public void TestRegisterMultipleControllers() {
            var registry = new HQRegistry();
            var controller1 = new HQController();
            var controller2 = new DummyModuleController();

            registry.RegisterController(controller1);
            registry.RegisterController(controller2);

            Assert.AreEqual(2, registry.Controllers.Count);
            Assert.True(registry.Controllers.ContainsKey(controller1.GetType()));
            Assert.True(registry.Controllers.ContainsKey(controller2.GetType()));
        }

        [Test]
        public void TestRegisterMultipleServices() {
            var registry = new HQRegistry();
            var service1 = new HQService();
            var service2 = new DummyModuleService();

            registry.RegisterService(service1);
            registry.RegisterService(service2);

            Assert.AreEqual(2, registry.Services.Count);
            Assert.True(registry.Services.ContainsKey(service1.GetType()));
            Assert.True(registry.Services.ContainsKey(service2.GetType()));
        }

        [Test]
        public void TestRegisterMultipleViews() {
            var registry = new HQRegistry();
            var view1 = new HQView();
            var view2 = new DummyModuleView();

            registry.RegisterView(view1);
            registry.RegisterView(view2);

            Assert.AreEqual(2, registry.Views.Keys.Count);
            Assert.True(registry.Views.ContainsKey(view1.GetType()));
            Assert.True(registry.Views.ContainsKey(view2.GetType()));
            Assert.True(registry.Views[view1.GetType()].Contains(view1));
            Assert.True(registry.Views[view2.GetType()].Contains(view2));
        }

        public void TestDuplicateControllers() {

        }

        public void TestDuplcateServices() {

        }

        public void TestDuplcateViews() {

        }
    }
}