using HQDotNet.Model;

namespace HQDotNet.Test {
    public class DummyModuleViewInvalid : HQView, IModelListener<DummyData> {

        //Invalid
        [HQInject]
        private DummyModuleController _controller;

        public bool HasController() {
            return _controller != null;
        }

        public string DisplayString { get; private set; }

        void IModelListener<DummyData>.OnModelUpdated(ref DummyData model) {
            System.Console.WriteLine("OnModelUpdated DummyModel: " + model.title);
        }
    }
}
