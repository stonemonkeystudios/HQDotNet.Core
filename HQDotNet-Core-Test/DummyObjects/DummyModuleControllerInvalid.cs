using HQDotNet.Model;

namespace HQDotNet.Test {
    public class DummyModuleControllerInvalid: HQController {

        //Invalid: Controllers are singletons
        [HQInject]
        private DummyModuleControllerInvalid _controller;

        //Invalid
        [HQInject]
        private DummyModuleViewInvalid _view;

        public bool HasView() {
            return _view != null;
        }

        public bool HasController() {
            return _controller != null;
        }
    }
}
