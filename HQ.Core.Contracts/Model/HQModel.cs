using System.Runtime.Serialization;
using HQ;

namespace HQ.Model {
    public class HQModel : HQObject, IExtensibleDataObject {

        public HQModel() {
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
