using HQDotNet.Model;


namespace HQDotNet {
    /// <summary>
    /// This is for automatic serialization of classes to subtypes.
    /// </summary>
    public abstract class HQCoreBehavior<TModel> : HQObject
        where TModel : HQBehaviorModel, new() {

        public TModel Model { get; protected set; }

        [HQInject]
        private HQSession _session;

        public HQCoreBehavior() {
            Model = new TModel();
            Model.Phase = HQPhase.Initialized;
        }

        public HQCoreBehavior(TModel state) {
            Model = state;
        }

        public virtual bool Shutdown() {
            Model.Phase = HQPhase.Shutdown;
            Model = null;
            return true;
        }

        public virtual bool Startup() {
            Model.Phase = HQPhase.Started;
            return true;
        }

        public virtual void Update() {
        }
    }
}