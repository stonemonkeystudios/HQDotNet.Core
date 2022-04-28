using HQDotNet.Model;

namespace HQDotNet.Test {
    public class DummyModuleView2 : HQView, IModelListener<DummyData>{

        [HQInject]
        private DummyModuleService _service;

        //Views can be Multitons
        [HQInject]
        private DummyModuleView _view;

        public string DisplayString { get; private set; }

        public bool HasService() {
            return _service != null;
        }

        public bool HasView() {
            return _view != null;
        }

        void IModelListener<DummyData>.OnModelUpdated(ref DummyData model) {
            DisplayString = model.title;
        }
    }
}
