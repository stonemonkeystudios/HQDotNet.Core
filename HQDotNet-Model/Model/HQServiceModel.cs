using System.Runtime.Serialization;

namespace HQDotNet.Model {
    public class HQServiceModel : HQCoreBehaviorModel {
        public virtual string Name { get; set; }

        public HQServiceModel() : base() {
            Name = GetType().Name;
        }
    }
}
