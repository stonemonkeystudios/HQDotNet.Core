using System;
using System.Threading;
using System.Threading.Tasks;

namespace HQDotNet.Test {
    public class DummyModuleService : HQService{
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
