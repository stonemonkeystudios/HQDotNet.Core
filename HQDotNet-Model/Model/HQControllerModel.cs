using System.Runtime.Serialization;

namespace HQDotNet.Model {
    public class HQControllerModel : HQCoreBehaviorModel {
        public virtual string Name { get; set; }

        public HQControllerModel() : base() {
            Name = GetType().Name;
        }
    }
}
