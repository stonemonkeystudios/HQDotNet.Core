using System.Threading.Tasks;

namespace HQDotNet.Test {
    public class DummyModuleController2: HQController {

        [HQInject]
        private DummyModuleService _service;

        [HQInject]
        private DummyModuleController _controller;

        public bool HasService() {
            return _service != null;
        }

        public bool HasController() {
            return _controller != null;
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
            Session.Dispatcher.Dispatch<IModelListener<DummyData>>(listener => listener.OnModelUpdated(data));
        }
    }
}
