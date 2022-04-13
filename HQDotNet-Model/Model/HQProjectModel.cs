namespace HQDotNet.Model {
    public class HQProjectModel : HQDataModel {
        public virtual string Name { get; set; }
        public virtual string Version { get; set; }

        public HQProjectModel() : base() {
            Name = "HQ.Module";
            Version = "v1";
        }
    }
}
