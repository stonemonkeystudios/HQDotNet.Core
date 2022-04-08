using System.Runtime.Serialization;
using HQDotNet;

namespace HQDotNet.Model {
    public class HQDataModel : HQObject, IExtensibleDataObject {

        public HQDataModel() {
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
