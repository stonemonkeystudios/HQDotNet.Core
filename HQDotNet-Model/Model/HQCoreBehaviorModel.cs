using System.Runtime.Serialization;

namespace HQDotNet.Model {
    public class HQCoreBehaviorModel : HQDataModel {

        /// <summary>
        /// Current lifecycle phase of this item.
        /// </summary>
        public HQPhase Phase { get; set; }

        public HQCoreBehaviorModel() {
            Phase = HQPhase.Shutdown;
        }
    }
}
