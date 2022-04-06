using System.Collections.Generic;
using System.Threading;
using HQ.Model;

namespace HQ.Service {
    /// <summary>
    /// An HQ Service is intended to run on a worker thread and report its results back to the controller that spawned it, using background worker.
    /// Service types should be registered with HQ, but they are not behaviors
    /// </summary>
    public class HQService<TBehaviorModel> : HQSingletonBehavior<TBehaviorModel> where TBehaviorModel : HQServiceModel, new() {
    }
}
