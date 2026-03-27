using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace HQDotNet
{
    public sealed class HQDispatcher : HQCoreBehavior, IHQDispatcher{

        //It would be nice for these to also be immediately injectable.[HQInject]
        private IHQRegistry _registry;
        public void SetRegistry(IHQRegistry registry) {
            _registry = registry;
        }

        public void RegisterDispatchListenersForObject(object listenerObject){
            //Iterate all listener types on the given object
            Type listenerType = listenerObject.GetType();
            Type baseDispatchListenerType = typeof(IDispatchListener);

            Type[] interfaceTypes = listenerType.FindInterfaces(DispatchListenerFilter, listenerObject);
            foreach(Type interfaceType in interfaceTypes) {
                var listener = listenerObject;
                _registry.BindListener(interfaceType, (IDispatchListener)listenerObject);
            }
        }

        public void UnregisterDispatchListenerInterface<TListener>(TListener behavior) where TListener : IDispatchListener {
            _registry.UnbindBehaviorListenerForObject(behavior);
        }

        public List<TListener> GetListeners<TListener>() where TListener : IDispatchListener{
            return _registry.GetDispatchListenersForType<TListener>();
        }

        public void UnregisterDispatchListenersForObject(object listenerObject) {
            //Iterate all listener types on the given object
            Type listenerType = listenerObject.GetType();
            Type baseDispatchListenerType = typeof(IDispatchListener);
            if (!baseDispatchListenerType.IsAssignableFrom(listenerType)) {
                return;
            }

            Type[] interfaceTypes = listenerType.FindInterfaces(DispatchListenerFilter, listenerObject);
            foreach (Type interfaceType in interfaceTypes) {
                var listener = listenerObject;
                _registry.UnbindBehaviorListenerForObject(interfaceType, (IDispatchListener)listenerObject);
            }
        }

        /// <summary>
        /// Dispatches to currently registered listeners immediately and synchronously on the calling thread.
        /// </summary>
        /// <typeparam name="TDispatchListener">The listener interface type to dispatch to.</typeparam>
        /// <param name="dispatchMessage">The action executed for each registered listener right away during this call.</param>
        /// <remarks>
        /// Current behavior is immediate invocation; no queueing or delayed execution is performed here.
        /// </remarks>
        public void Dispatch<TDispatchListener>(Action<TDispatchListener> dispatchMessage) where TDispatchListener : IDispatchListener {
            var listeners = GetListeners<TDispatchListener>();
            foreach(var listener in listeners) {
                dispatchMessage(listener);
            }
        }

        private bool DispatchListenerFilter(Type type, object criteriaObject) {

            Type listenerType = criteriaObject.GetType();
            Type baseDispatchListenerType = typeof(IDispatchListener);

            bool valid = baseDispatchListenerType.IsAssignableFrom(listenerType);
            return valid;
        }

        public override void LateUpdate() {

            base.LateUpdate();
        }

    }
}
