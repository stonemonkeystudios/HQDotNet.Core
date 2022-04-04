namespace HQ.Model {
    public class HQProjectModel : HQModel {
        public virtual string Name { get; set; }
        public virtual string Version { get; set; }

        public HQProjectModel() : base() {
            Name = "HQ.Module";
            Version = "v1";
        }
    }
}
