using System;
using System.Collections.Generic;

namespace HQDotNet {
    public interface IHQDispatcher {
        void SetRegistry(IHQRegistry registry);
        void RegisterDispatchListenersForObject(object listenerObject);
        void UnregisterDispatchListenerInterface<TListener>(TListener behavior) where TListener : IDispatchListener;
        List<TListener> GetListeners<TListener>() where TListener : IDispatchListener;
        void UnregisterDispatchListenersForObject(object listenerObject);
        void Dispatch<TDispatchListener>(Action<TDispatchListener> dispatchMessage) where TDispatchListener : IDispatchListener;
    }
}
