using System.Runtime.Serialization;
using HQDotNet;

namespace HQDotNet.Model {

    public class HQDataModel : HQObject, IExtensibleDataObject {

        //Use IsDirty in combination with Dispatch Attribute and dispatcher to automatically dispatch updates
        public bool IsDirty { get;}

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
