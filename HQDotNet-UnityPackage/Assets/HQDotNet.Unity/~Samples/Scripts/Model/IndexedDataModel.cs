namespace HQDotNet {
    public struct IndexedDataModel<TObject> where TObject : new() {
        public int index;
        public TObject data;

        public IndexedDataModel(int index, ref TObject data) {
            this.index = index;
            this.data = data;
        }
    }
}