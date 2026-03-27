using System;
using System.Collections.Generic;

namespace HQDotNet {
    public interface IHQRegistry {
        Dictionary<Type, HQController> Controllers { get; }
        Dictionary<Type, HQService> Services { get; }
        Dictionary<Type, List<HQView>> Views { get; }

        bool RegisterController<TBehavior>(TBehavior controller) where TBehavior : HQController, new();
        bool RegisterService<TBehavior>(TBehavior service) where TBehavior : HQService, new();
        bool RegisterView<TBehavior>(TBehavior view) where TBehavior : HQView, new();
        void Unregister(HQCoreBehavior behavior);

        void BindListener(Type type, object listenerObject);
        void UnbindBehaviorListenerForObject<TListenerBehavior>(TListenerBehavior behavior) where TListenerBehavior : IDispatchListener;
        void UnbindBehaviorListenerForObject(Type listenerType, IDispatchListener listener);
        List<TDispatchListener> GetDispatchListenersForType<TDispatchListener>() where TDispatchListener : IDispatchListener;
    }
}
