using HQDotNet.Model;

namespace HQDotNet.Test {
    public class DummyModuleViewInvalid : HQView, IModelListener<DummyData> {

        //Invalid
        [HQInject]
        private DummyModuleServiceInvalid _service;

        //Invalid
        [HQInject]
        private DummyModuleViewInvalid _view;

        public bool HasService() {
            return _service != null;
        }

        public bool HasView() {
            return _view != null;
        }

        public string DisplayString { get; private set; }

        void IModelListener<DummyData>.OnModelUpdated(ref DummyData model) {
            System.Console.WriteLine("OnModelUpdated DummyModel: " + model.title);
        }
    }
}
