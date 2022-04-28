
using NUnit.Framework;
using System.Threading.Tasks;

namespace HQDotNet.Test {

    public class HQSessionTest {

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
        /// <returns></returns>
        [Test]
        public async Task SimpleSessionTest() {
            string dummyTitleString = "DummyTitle";
            var controller = _session.RegisterController<DummyModuleController>();
            var view = _session.RegisterView<DummyModuleView>();
            _session.RegisterService<DummyModuleService>();

            //View's display string is initially null
            Assert.IsNull(view.DisplayString);

            //Query a test service that waits 50ms and then changes the data
            //When the service is complete, a Dispatch is sent to all IModelData<DummyData>
            //View is a listener, so will automatically receive the update
            //Awaiting for simplicity sake, but this is async.
            await controller.QueryDummyDelayedServiceForData(dummyTitleString);

            //Dispatches currently send in late update, to sync with main thread.
            _session.LateUpdate();

            //View magically has the new display string
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
                //Dispatches currently send in late update, to sync with main thread.
                _session.LateUpdate();
                Assert.AreEqual(dummyTitleString + "_" + i, view.DisplayString);
            }

        }

        public void UnregisterControllerTest() {

        }
    }
}