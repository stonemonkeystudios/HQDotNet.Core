using HQDotNet;

namespace HQDotNet.Model {
    public class HQViewModel : HQCoreBehaviorModel{

        public virtual string Name { get; set; }

        public HQViewModel() : base() {
            Name = GetType().Name;
        }

    }
}
