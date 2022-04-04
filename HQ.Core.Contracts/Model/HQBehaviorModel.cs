using System.Runtime.Serialization;

namespace HQ.Model {
    public class HQBehaviorModel : HQModel {

        /// <summary>
        /// Current lifecycle phase of this item.
        /// </summary>
        public HQPhase Phase { get; set; }

        public HQBehaviorModel() {
            Phase = HQPhase.Shutdown;
        }
    }
}
