using HQDotNet.Model;

namespace HQDotNet {
    public class HQController<TModel> : HQSingletonBehavior<TModel>
        where TModel : HQControllerModel, new() {

    }
}
