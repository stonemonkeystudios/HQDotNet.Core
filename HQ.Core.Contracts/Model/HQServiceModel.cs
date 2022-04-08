using System.Runtime.Serialization;

namespace HQDotNet.Model {
    public class HQServiceModel : HQBehaviorModel {
        public virtual string Name { get; set; }

        public HQServiceModel() : base() {
            Name = GetType().Name;
        }
    }
}
