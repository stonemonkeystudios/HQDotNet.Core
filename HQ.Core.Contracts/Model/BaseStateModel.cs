using System.Runtime.Serialization;

namespace HQ.Contracts {
    public class BaseStateModel : IExtensibleDataObject {

        /// <summary>
        /// Current lifecycle phase of this item.
        /// </summary>
        public HQPhase Phase { get; set; }

        public BaseStateModel() {
            Phase = HQPhase.Shutdown;
        }

        //For future-proofing
        protected ExtensionDataObject _extensionDataObject;

        public ExtensionDataObject ExtensionData {
            get {
                return _extensionDataObject;
            }
            set {
                _extensionDataObject = value;
            }
        }
    }
}
