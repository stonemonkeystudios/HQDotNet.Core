using System;
using System.Collections.Generic;
using System.Text;
using HQDotNet.Model;

namespace HQDotNet {
    public interface IBehaviorListener : IDispatchListener {
        void PhaseUpdated(HQPhase phase);

    }
}
