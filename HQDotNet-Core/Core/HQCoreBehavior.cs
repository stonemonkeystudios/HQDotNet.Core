using HQDotNet.Model;


namespace HQDotNet {

    public abstract class HQCoreBehavior: HQObject{

        protected HQCoreBehaviorModel _model = new HQCoreBehaviorModel();

        public HQCoreBehaviorModel Model { get { return _model; } protected set { _model = value; } }

        [HQInject]
        protected HQSession session;

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

        public virtual void LateUpdate() {
        }
    }
}