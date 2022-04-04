using HQ.Model;


namespace HQ {
    /// <summary>
    /// This is for automatic serialization of classes to subtypes.
    /// </summary>
    public abstract class HQBehavior<TModel> : HQObject
        where TModel : HQBehaviorModel, new(){

        public TModel Model { get; protected set; }

        public HQBehavior() {
            Model = new TModel();
            Model.Phase = HQPhase.Initialized;
        }

        public HQBehavior(TModel state) {
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