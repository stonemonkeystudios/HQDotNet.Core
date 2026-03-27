namespace HQDotNet {
    public interface IHQScope {
        TBehavior RegisterController<TBehavior>() where TBehavior : HQController, new();
        TBehavior RegisterService<TBehavior>() where TBehavior : HQService, new();
        TBehavior RegisterView<TBehavior>() where TBehavior : HQView, new();

        void RegisterObjectOnlyForDispatch(object obj);
        void UnregisterNonHQBehaviorDispatch(object obj);
        void Unregister(HQCoreBehavior behavior);
    }
}
