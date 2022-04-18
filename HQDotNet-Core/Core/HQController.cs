using HQDotNet.Model;

namespace HQDotNet {

    public class HQController : HQSingletonBehavior{

        [HQInject]
        protected HQDispatcher _dispatcher;

    }
}
