using System.Threading.Tasks;

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

        public async Task QueryDummyDelayedServiceForData(string newTitle) {
            await _service.AsyncDummyDelayedServiceCall(newTitle, DataReceived);
        }

        public async Task QueryDummyImmediateServiceForData(string newTitle) {
            await _service.AsyncDummyImmediateServiceCall(newTitle, DataReceived);
        }

        private void DataReceived(DummyData data) {
            DispatchOnModelUpdated(data);
        }

        private void DispatchOnModelUpdated(DummyData data) {
            HQDispatcher.DispatchMessageDelegate<IModelListener<DummyData>> dispatchMessage =
                (IModelListener<DummyData> modelListener) => {
                    return () => modelListener.OnModelUpdated(ref data);
                };
            dispatcher.Dispatch(dispatchMessage);
        }
    }
}
