using System;
using System.Collections.Generic;
using System.Linq;
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
            _registry = null;
        }

        [Test]
        public void InstantiationTestSimple() {
            Assert.NotNull(_injector);
        }

        /*
         * Valid
         * Y Controller->Controller
         * Y Service->Controller
         * Y Service->View
         * Y View->View
         * 
         * Invalid
         * N Controller->Service
         * N Controller->View
         * N Service->Service
         * N View->Controller
         * N View->Service
         */

        [Test]
        public void ValidInjection_ControllerIntoController() {
            var controller1 = new DummyModuleController();
            var controller2 = new DummyModuleController2();

            _registry.RegisterController(controller1);
            _registry.RegisterController(controller2);

            _injector.Inject(controller1);
            _injector.Inject(controller2);

            Assert.True(controller1.HasController());
            Assert.True(controller2.HasController());
        }

        [Test]
        public void ValidInjection_ServiceIntoController() {
            var controller = new DummyModuleController();
            var service = new DummyModuleService();

            _registry.RegisterController(controller);
            _registry.RegisterService(service);

            _injector.Inject(controller);
            _injector.Inject(service);

            Assert.True(controller.HasService());
        }

        [Test]
        public void ValidInjection_ServiceIntoView() {
            var view = new DummyModuleView();
            var service = new DummyModuleService();

            _registry.RegisterView(view);
            _registry.RegisterService(service);

            _injector.Inject(view);
            _injector.Inject(service);

            Assert.True(view.HasService());

        }

        [Test]
        public void ValidInjection_ViewIntoView() {
            var view = new DummyModuleView();
            var view2 = new DummyModuleView2();

            _registry.RegisterView(view);
            _registry.RegisterView(view2);

            _injector.Inject(view);
            _injector.Inject(view2);

            Assert.True(view.HasView());
            Assert.True(view2.HasView());
        }

        [Test]
        public void InvalidInjection_Controller() {
            var controller = new DummyModuleControllerInvalid();
            _registry.RegisterController(controller);
            Assert.Throws<HQInjectionException>(() => _injector.Inject(controller));
        }

        [Test]
        public void InvalidInjection_Service() {
            var service = new DummyModuleServiceInvalid();
            _registry.RegisterService(service);
            Assert.Throws<HQInjectionException>(() => _injector.Inject(service));
        }

        [Test]
        public void InvalidInjection_View() {
            var view = new DummyModuleViewInvalid();
            _registry.RegisterView(view);
            Assert.Throws<HQInjectionException>(() => _injector.Inject(view));
        }

        [Test]
        public void TestPermutationsInterwoven() {
            var behaviors = new HQCoreBehavior[] {
                new DummyModuleController(),
                new DummyModuleController2(),
                new DummyModuleService(),
                new DummyModuleView(),
                new DummyModuleView2() }
            ;
            //Use two lists so that order 
            var permutationsList = RecursiveHeapPermutation(behaviors, behaviors.Length, new List<HQCoreBehavior[]>());
            Console.WriteLine("Testing " + permutationsList.Count + " permutations.");

            //It should not matter what order you register and inject
            //Everything should properly be injected by the end of frame, if created in same frame
            foreach (var permutation in permutationsList) {
                //For every behavior, register then immediately inject
                //This is how it works in HQSession
                Setup();
                RegisterAndInjectBehaviors(permutation);
                AssertInjections(permutation);
                Teardown();
            }
        }

        [Test]
        public void TestPermutationsInjectThenRegister() {
            var behaviors = new HQCoreBehavior[] {
            new DummyModuleController(),
            new DummyModuleController2(),
            new DummyModuleService(),
            new DummyModuleView(),
            new DummyModuleView2() };

            var permutationsList = RecursiveHeapPermutation(behaviors, behaviors.Length, new List<HQCoreBehavior[]>());
            Console.WriteLine("Testing " + permutationsList.Count + " permutations.");

            //Iterate a second time with 
            foreach (var permutation in permutationsList) {

                //First register all behaviors, then inject all behaviors
                Setup();
                RegisterBehaviors(permutation);
                InjectBehaviors(permutation);
                AssertInjections(permutation);
                Teardown();
            }
        }

        private void RegisterAndInjectBehaviors(HQCoreBehavior[] behaviors) {
            foreach (var behavior in behaviors) {
                BehaviorCategory cat = HQRegistry.GetBehaviorCategory(behavior.GetType());
                switch (cat) {
                    case BehaviorCategory.Controller:
                        _registry.RegisterController((HQController)behavior);
                        break;
                    case BehaviorCategory.View:
                        _registry.RegisterView((HQView)behavior);
                        break;
                    case BehaviorCategory.Service:
                        _registry.RegisterService((HQService)behavior);
                        break;
                }
                _injector.Inject(behavior);
            }
        }

        private void RegisterBehaviors(HQCoreBehavior[] behaviors) {
            foreach (var behavior in behaviors) {
                BehaviorCategory cat = HQRegistry.GetBehaviorCategory(behavior.GetType());
                switch (cat) {
                    case BehaviorCategory.Controller:
                        _registry.RegisterController((HQController)behavior);
                        break;
                    case BehaviorCategory.View:
                        _registry.RegisterView((HQView)behavior);
                        break;
                    case BehaviorCategory.Service:
                        _registry.RegisterService((HQService)behavior);
                        break;
                }
            }
        }

        private void InjectBehaviors(HQCoreBehavior[] behaviors) {
            foreach (var behavior in behaviors) {
                _injector.Inject(behavior);
            }
        }

        // Generating permutation using Heap Algorithm
        static List<HQCoreBehavior[]> RecursiveHeapPermutation(HQCoreBehavior[] behaviors, int size, List<HQCoreBehavior[]> result) {
            // if size becomes 1 then prints the obtained
            // permutation
            if (size == 1) {
                result.Add((new List<HQCoreBehavior>(behaviors)).ToArray());
                return result;
            }
                //printArr(behaviors, n);

            for (int i = 0; i < size; i++) {
                RecursiveHeapPermutation(behaviors, size - 1, result);

                // if size is odd, swap 0th i.e (first) and
                // (size-1)th i.e (last) element
                if (size % 2 == 1) {
                    HQCoreBehavior temp = behaviors[0];
                    behaviors[0] = behaviors[size - 1];
                    behaviors[size - 1] = temp;
                }

                // If size is even, swap ith and
                // (size-1)th i.e (last) element
                else {
                    HQCoreBehavior temp = behaviors[i];
                    behaviors[i] = behaviors[size - 1];
                    behaviors[size - 1] = temp;
                }

                //result.Add((new List<HQCoreBehavior>(behaviors)).ToArray());
            }
            return result;
        }

        private void AssertInjections(HQCoreBehavior[] behaviors) {

            foreach (HQCoreBehavior behavior in behaviors) {
                if (typeof(DummyModuleController).IsAssignableFrom(behavior.GetType())) {
                    Assert.IsTrue((behavior as DummyModuleController).HasController());
                    Assert.IsTrue((behavior as DummyModuleController).HasService());
                }
                else if (typeof(DummyModuleController2).IsAssignableFrom(behavior.GetType())) {
                    Assert.IsTrue((behavior as DummyModuleController2).HasController());
                    Assert.IsTrue((behavior as DummyModuleController2).HasService());
                }
                else if (typeof(DummyModuleView).IsAssignableFrom(behavior.GetType())) {
                    Assert.IsTrue((behavior as DummyModuleView).HasView());
                    Assert.IsTrue((behavior as DummyModuleView).HasService());
                }
                else if (typeof(DummyModuleView2).IsAssignableFrom(behavior.GetType())) {
                    Assert.IsTrue((behavior as DummyModuleView2).HasView());
                    Assert.IsTrue((behavior as DummyModuleView2).HasService());
                }
            }
        }
    }
}