using HQDotNet.Model;

namespace HQDotNet.Test {
    public class DummyModuleController: HQController {

        [HQInject]
        private DummyModuleService _service;

        [HQInject]
        private DummyModuleControllerInvalid _controller2;

        public bool HasService() {
            return _service != null;
        }

        public bool HasController2() {
            return _controller2 != null;
        }

        public void QueryDummyServiceForData() {
            _service.AsyncDummyServiceCall(DataReceived);
        }

        private void DataReceived(DummyData data) {
            DispatchOnModelUpdated(data);
        }

        private void DispatchOnModelUpdated(DummyData data) {
            HQDispatcher.DispatchMessageDelegate<IModelListener<DummyData>> dispatchMessage =
                (IModelListener<DummyData> modelListener) => {
                    return () => modelListener.OnModelUpdated(data);
                };
            _dispatcher.Dispatch(dispatchMessage);
        }
    }
}
