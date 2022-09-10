using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace HQDotNet
{
    /*
     * TODO: If we want to be able to thread behaviors, Dispatching should send on the main thread
     * Or store until a given syncronization point.
     * In this case, Dispatcher could actually be a behavior and syncing would be done in the update function
     * 
     * TODO: Dispatcher needs 
     * 
     * 
     * 
     */

    public sealed class HQDispatcher : HQCoreBehavior{

        //TODO: Dispatcher should queue up all dispatches to be executed in a (new) LateUpdate method.
        //Any other threads that need to dispatch should register here and will be dispatched later
        //What sort of design implications does this have?

        //It would be nice for these to also be immediately injectable.[HQInject]
        private HQRegistry _registry;
        public void SetRegistry(HQRegistry registry) {
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