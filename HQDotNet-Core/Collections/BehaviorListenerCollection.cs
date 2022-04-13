using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using HQDotNet.Model;

namespace HQDotNet {
    public class BehaviorListenerCollection<TBehaviorListener> : DispatchListenerCollection<TBehaviorListener>, IBehaviorListener where TBehaviorListener : IBehaviorListener{

        void IBehaviorListener.PhaseUpdated(HQPhase phase) {
            this.ForEach((listener) => listener.PhaseUpdated(phase));
        }
    }
}
