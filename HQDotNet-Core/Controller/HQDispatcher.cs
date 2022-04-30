using System;
using System.Collections.Concurrent;
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

        private ConcurrentQueue<Action> _pendingDispatch;

        public HQDispatcher() : base() {
            _pendingDispatch = new ConcurrentQueue<Action>();
        }

        public void SetRegistry(HQRegistry registry) {
            _registry = registry;
        }

        public void RegisterDispatchListenersForObject(object listenerObject){
            //Iterate all listener types on the given object
            Type listenerType = listenerObject.GetType();
            Type baseDispatchListenerType = typeof(IDispatchListener);

            if (!baseDispatchListenerType.IsAssignableFrom(listenerType)) {
                return;
            }

            Type[] interfaceTypes = listenerType.FindInterfaces(DispatchListenerFilter, listenerObject);
            foreach(Type interfaceType in interfaceTypes) {
                var listener = listenerObject;
                _registry.BindListener(interfaceType, (IDispatchListener)listenerObject);
            }
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

        public void UnregisterDispatchListenerInterface<TListener>(TListener listener)
            where TListener : HQObject, IDispatchListener {
            _registry.UnbindAllDispatchListenersForType(typeof(TListener));
        }

        public delegate Action DispatchMessageDelegate<TDispatchListener>(TDispatchListener dispatchListener);
        public void Dispatch<TDispatchListener>(DispatchMessageDelegate<TDispatchListener> dispatchMessage) where TDispatchListener : IDispatchListener {

            lock (_pendingDispatch) {
                var listeners = _registry.GetDispatchListenersForType<TDispatchListener>();
                foreach (var listener in listeners) {
                    _pendingDispatch.Enqueue(dispatchMessage((TDispatchListener)listener));
                }
            }
        }

        //[MethodImpl(MethodImplOptions.Synchronized)]
        private void ExecuteDispatchQueue() {
            lock (_pendingDispatch) {
                Action dequeuedAction;
                if (_pendingDispatch != null) {
                    while (_pendingDispatch.TryDequeue(out dequeuedAction)) {
                        dequeuedAction();
                    }
                }
            }
        }

        private bool DispatchListenerFilter(Type type, object criteriaObject) {
            return typeof(IDispatchListener).IsAssignableFrom(type);
        }

        private static bool ValidateDispatcherRules(Type interfaceType, Type behaviorType) {
            /*
             * We can use this before registering a behavior to validate Interfaces
             *      -Views can't implement IServiceListener for example
             */

            return true;
        }

        public override void LateUpdate() {

            ExecuteDispatchQueue();

            base.LateUpdate();
        }

    }
}