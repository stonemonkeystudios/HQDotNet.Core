namespace HQDotNet {
    public interface IHQRuntimeContext : IHQScope {
        System.DateTime StartDate { get; }
        System.TimeSpan TimeSinceStarted { get; }
        IHQDispatcher Dispatcher { get; }

        TBehavior GetService<TBehavior>() where TBehavior : HQService;
        TBehavior GetController<TBehavior>() where TBehavior : HQController;
    }
}
