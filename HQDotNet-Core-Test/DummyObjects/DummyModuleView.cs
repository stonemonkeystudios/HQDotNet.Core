using HQDotNet.Model;

namespace HQDotNet.Test {
    public class DummyModuleView : HQView, IModelListener<DummyData>{

        [HQInject]
        private DummyModuleService _service;

        //Views can be Multitons
        [HQInject]
        private DummyModuleView2 _view;

        public string DisplayString { get; private set; }

        public bool HasService() {
            return _service != null;
        }

        public bool HasView() {
            return _view != null;
        }

        void IModelListener<DummyData>.OnModelUpdated(DummyData model) {
            DisplayString = model.title;
        }
    }
}
