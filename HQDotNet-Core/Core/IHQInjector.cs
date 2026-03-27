namespace HQDotNet {
    public interface IHQInjector {
        void SetRegistry(IHQRegistry registry);
        bool Inject(HQCoreBehavior behavior);
        void UninjectBehavior(HQCoreBehavior behavior);
        void ValidateInjectionRules(HQCoreBehavior behaviorToInject);
    }
}
