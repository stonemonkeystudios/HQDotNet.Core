using System.Runtime.Serialization;

namespace HQ.Model {
    public class HQControllerModel : HQBehaviorModel {
        public virtual string Name { get; set; }

        public HQControllerModel() : base() {
            Name = GetType().Name;
        }
    }
}
