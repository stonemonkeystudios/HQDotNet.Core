using HQDotNet.Model;

namespace HQDotNet.Test {
    public class DummyModuleControllerInvalid: HQController {

        //Invalid
        [HQInject]
        private DummyModuleViewInvalid _view;

        public bool HasView() {
            return _view != null;
        }
    }
}
