using HQDotNet;

namespace HQDotNet.Model {
    public class HQViewModel<TDataModel> : HQCoreBehaviorModel where TDataModel : HQDataModel{

        public TDataModel DataModel { get; protected set; }

        //TODO: ViewModel is set up with the data model it needs to render
        //So a view model itself needs the model to render, as views are independent?


        //TODO: just move names to behaviors I think
        public virtual string Name { get; set; }

        public HQViewModel() : base() {
            Name = GetType().Name;
        }
    }
}
