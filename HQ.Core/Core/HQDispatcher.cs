using System;
using System.Runtime;
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

    public class HQDispatcher : HQBehavior<HQBehaviorModel> {
        private Dictionary<Type, IDispatchListener> _dispatchMap = new Dictionary<Type, IDispatchListener>();
        public void Register<TCollection, TListenerType>() 
            where TCollection : DispatchListenerCollection<TListenerType>, TListenerType, new()
            where TListenerType : IDispatchListener {

            var collection = new DispatchListenerCollection<TListenerType>();
            Type listenerType = typeof(TListenerType);
            if (_dispatchMap.ContainsKey(listenerType))
                throw new HQException("Dispatcher already contains a listener for type.");

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

        private static bool ValidateDispatcherRules(Type interfaceType, Type behaviorType) {
            /*
             * We can use this before registering a behavior to validate Interfaces
             *      -Views can't implement IServiceListener for example
             */

            return true;
        }
    }
}