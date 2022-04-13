using HQDotNet.Model;
using NUnit.Framework;

namespace HQDotNet.Test {
    public class HQRegistryTest {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public void RegisterControllerSimple() {
            HQRegistry registry = new HQRegistry();

            Assert.IsNotNull(registry.Controllers);
            Assert.AreEqual(0, registry.Controllers.Keys.Count);

            HQController<HQControllerModel> controller = new HQController<HQControllerModel>();
            registry.RegisterController(controller);
            Assert.AreEqual(1, registry.Controllers.Keys.Count);

            Assert.True( registry.Controllers.ContainsKey(controller.Model.GetType()));
            Assert.AreEqual(controller, registry.Controllers[controller.Model.GetType()]);



            //registry.RegisterController<HQController<HQControllerModel>, HQControllerModel>(controller);




            Assert.Pass();
        }
    }
}