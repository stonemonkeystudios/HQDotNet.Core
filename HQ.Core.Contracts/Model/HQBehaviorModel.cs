using System.Runtime.Serialization;

namespace HQDotNet.Model {
    public class HQBehaviorModel : HQDataModel {

        /// <summary>
        /// Current lifecycle phase of this item.
        /// </summary>
        public HQPhase Phase { get; set; }

        public HQBehaviorModel() {
            Phase = HQPhase.Shutdown;
        }
    }
}
