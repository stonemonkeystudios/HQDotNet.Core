using HQ.Model;

namespace HQ {
    public abstract class HQSingletonBehavior<TModel> : HQBehavior<TModel>
        where TModel : HQBehaviorModel, new() {

    }
}
