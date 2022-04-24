using HQDotNet.Model;

namespace HQDotNet.Test {
    public class DummyModuleView : HQView, IModelListener<DummyData>{

        [HQInject]
        private DummyModuleController _controller;

        public string DisplayString { get; private set; }

        public bool HasController() {
            return _controller != null;
        }

        void IModelListener<DummyData>.OnModelUpdated(ref DummyData model) {
            //System.Console.WriteLine("OnModelUpdated DummyModel: " + model.title);
            DisplayString = model.title;
        }
    }
}
