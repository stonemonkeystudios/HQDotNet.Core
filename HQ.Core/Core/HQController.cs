using HQDotNet.Model;

namespace HQDotNet {
    public abstract class HQController<TModel> : HQCoreBehavior<TModel>
        where TModel : HQControllerModel, new() {

    }
}
