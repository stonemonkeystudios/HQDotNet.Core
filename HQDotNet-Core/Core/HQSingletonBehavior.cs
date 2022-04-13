using HQDotNet.Model;

namespace HQDotNet
{
    public class HQSingletonBehavior<TModel> : HQCoreBehavior<TModel> where TModel : HQCoreBehaviorModel, new() {
    }
}
