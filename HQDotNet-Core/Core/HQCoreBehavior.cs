using HQDotNet.Model;


namespace HQDotNet {

    /// <summary>
    /// Base class for all stateful behaviors within the HQDotNet ecosystem
    /// These are behaviors that can be registered with an HQSession
    /// </summary>
    public abstract class HQCoreBehavior: HQObject{
        public HQPhase Phase { get; protected set; }

        public HQSession Session { get; private set; } //The session this behavior lives in

        public void SetSession(HQSession session) {
            Session = session;
        }

        /// <summary>
        /// Shuts down this behavior and unregisters itself from a session
        /// </summary>
        /// <returns></returns>
        public virtual bool Shutdown() {
            Phase = HQPhase.Shutdown;
            return true;
        }

        public virtual bool Startup() {
            Phase = HQPhase.Started;
            return true;
        }

        public virtual void Update() {
        }

        public virtual void LateUpdate() {
        }
    }
}