using System.Runtime.Serialization;

namespace HQ.Model {
    public class HQViewModel : HQBehaviorModel {
        public virtual string Name { get; set; }

        public HQViewModel() : base() {
            Name = GetType().Name;
        }
    }
}
