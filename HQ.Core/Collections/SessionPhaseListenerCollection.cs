using HQ.Model;

namespace HQ {
    public class SessionPhaseListenerCollection : BehaviorListenerCollection<ISessionListener>, ISessionListener{

        void IBehaviorListener.PhaseUpdated(HQPhase phase) {
            this.ForEach((listener) => listener.PhaseUpdated(phase));
        }
    }
}
