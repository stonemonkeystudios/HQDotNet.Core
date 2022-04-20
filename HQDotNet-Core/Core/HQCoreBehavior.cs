using HQDotNet.Model;


namespace HQDotNet {

    /// <summary>
    /// Base class for all stateful behaviors within the HQDotNet ecosystem
    /// These are behaviors that can be registered with an HQSession
    /// </summary>
    public abstract class HQCoreBehavior: HQObject{
        public HQPhase Phase { get; protected set; }

        [HQInject]
        protected HQSession session; //The session this behavior lives in

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