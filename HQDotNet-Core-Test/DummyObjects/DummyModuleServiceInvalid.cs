using System;
using System.Threading;
using System.Threading.Tasks;

namespace HQDotNet.Test {
    public class DummyModuleServiceInvalid : HQService{

        //Invalid
        [HQInject]
        private DummyModuleControllerInvalid _controller;

        //Invalid
        [HQInject]
        private DummyModuleServiceInvalid _service;

        //Invalid
        [HQInject]
        private DummyModuleViewInvalid _view;

        public bool HasController() {
            return _controller != null;
        }

        public bool HasService() {
            return _service != null;
        }

        public bool HasView() {
            return _view != null;
        }


        public void AsyncDummyServiceCall(Action<DummyData> dataRetrievedCallback) {
            Busywork(dataRetrievedCallback);
        }

        private async void Busywork(Action<DummyData> dataRetrievedCallback) {
            await Task.Run(BackroundThreadBusyWork).ContinueWith((task) => dataRetrievedCallback);
        }

        private void BackroundThreadBusyWork() {
            Thread.Sleep(100);
        }
    }
}
