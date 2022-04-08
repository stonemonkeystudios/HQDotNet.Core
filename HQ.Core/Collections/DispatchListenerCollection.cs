using System;
using System.Collections.Generic;
using System.Text;

namespace HQDotNet {
    public class DispatchListenerCollection<TDispatchListener> : List<TDispatchListener>, IDispatchListener where TDispatchListener : IDispatchListener{

    }
}
