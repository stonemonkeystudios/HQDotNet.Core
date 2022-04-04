using System;
using System.Collections.Generic;
using System.Text;
using HQ.Model;

namespace HQ {
    public interface IBehaviorListener : IDispatchListener {
        void PhaseUpdated(HQPhase phase);

    }
}
