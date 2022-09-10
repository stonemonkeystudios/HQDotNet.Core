using System;
using System.Threading;
using System.Threading.Tasks;

namespace HQDotNet.Test {
    public class DummyModuleServiceInherited : DummyModuleService{
        public System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
        public async Task AsyncDummyDelayedServiceCall(string newTitle, Action<DummyData> dataRetrievedCallback) {
            await Busywork(newTitle, dataRetrievedCallback);
        }

        public async Task AsyncDummyImmediateServiceCall(string newTitle, Action<DummyData> dataRetrievedCallback) {
            await BusyworkImmediate(newTitle, dataRetrievedCallback);
        }

        private async Task Busywork(string newTitle, Action<DummyData> dataRetrievedCallback) {
            await Task.Run(BackroundThreadBusyWork).ContinueWith((task) => dataRetrievedCallback(new DummyData() { title = newTitle }));
        }

        private void BackroundThreadBusyWork() {
            Thread.Sleep(50);
        }

        private async Task BusyworkImmediate(string newTitle, Action<DummyData> dataRetrievedCallback) {
            await Task.Run(BackroundThreadBusyWorkImmediate).ContinueWith((task) => dataRetrievedCallback(new DummyData() { title = newTitle }));
        }

        private void BackroundThreadBusyWorkImmediate() {
        }
    }
}
