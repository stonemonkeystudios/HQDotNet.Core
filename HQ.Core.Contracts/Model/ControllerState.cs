using System.Runtime.Serialization;

namespace HQ.Contracts {
    public class ControllerState : BaseStateModel {
        public virtual string Name { get; set; }

        public ControllerState() : base() {
            Name = GetType().Name;
        }
    }
}
