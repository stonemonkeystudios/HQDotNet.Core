using System;
using System;
using System.Reflection;

using System.Collections.Generic;
using HQ.Model;

namespace HQ
{
    /*
     * TODO: If we want to be able to thread behaviors, Dispatching should send on the main thread
     * Or store until a given syncronization point.
     * In this case, Dispatcher could actually be a behavior and syncing would be done in the update function
     */

    public class HQDispatcher : HQSingletonBehavior<HQBehaviorModel> {

        [HQInject]
        private HQRegistry _registry;
        
        public void RegisterListener<TListenerType>(TListenerType listener) 
            where TListenerType : HQObject, IDispatchListener {
            _registry.BindListener(listener);
        }

        public void RegisterListeners(HQObject listenerObject){
            //Iterate all listener types on the given object
            Type listenerType = listenerObject.GetType();
            Type baseDispatchListenerType = typeof(IDispatchListener);
            if (!baseDispatchListenerType.IsAssignableFrom(listenerType)) {
                return;
            }

            Type[] interfaceTypes = listenerType.FindInterfaces(DispatchListenerFilter, listenerObject);
            foreach(Type interfaceType in interfaceTypes) {
                var listener = listenerObject;
                _registry.BindListener((IDispatchListener)listenerObject);
            }
        }

        public void Unregister<TListenerType>() where TListenerType : IDispatchListener {

        }

        public TDispatchListener Dispatch<TDispatchListener>() where TDispatchListener : IDispatchListener {
            Type listenerType = typeof(TDispatchListener);
            if (!_dispatchMap.ContainsKey(listenerType)) {
                throw new HQException("No listeners registered for type '" + listenerType + "'");
            }

            var collection = _dispatchMap[listenerType];
            if (listenerType.IsAssignableFrom(collection.GetType())) {
                return (TDispatchListener)_dispatchMap[listenerType];
            }

            throw new HQException("Collection for type '" + listenerType.Name + "' should itself implement " + listenerType.Name);
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
    }
}