using System;
using System.Runtime;
using System.Reflection;

using System.Collections.Generic;
using HQ.Contracts;

namespace HQ
{
    /*
     * TODO: If we want to be able to thread behaviors, Dispatching should send on the main thread
     * Or store until a given syncronization point.
     * In this case, Dispatcher could actually be a behavior and syncing would be done in the update function
     */

    public class HQDispatcher : HQStateBehavior<BaseStateModel> {
        private Dictionary<Type, DispatchListenerCollection<IDispatchListener>> _dispatchMap = new Dictionary<Type, DispatchListenerCollection<IDispatchListener>>();

        public void RegisterBehavior(HQStateBehavior behavior) {
            Type behaviorType = behavior.GetType();

            Type[] interfaces = behaviorType.FindInterfaces(DispatchListenerFilter, behavior);
            foreach(Type type in interfaces) {
                if (type == typeof(IDispatchListener))
                    continue;

                if (!_dispatchListeners.ContainsKey(type))
                    _dispatchListeners.Add(type, new List<IDispatchListener>());

                IDispatchListener listener = (IDispatchListener)behavior;

                if (!_dispatchListeners[type].Contains(listener)) {
                    _dispatchListeners[type].Add(listener);
                }
            }
        }

        public void UnregisterBehavior(HQStateBehavior behavior) {
            Type behaviorType = behavior.GetType(); 

            Type[] interfaces = behaviorType.FindInterfaces(DispatchListenerFilter, behavior);
            foreach (Type type in interfaces) {
                if (_dispatchListeners.ContainsKey(type)) {
                    List<IDispatchListener> typeListeners = _dispatchListeners[type];

                    for(int i = typeListeners.Count - 1; i >= 0; i--) {
                        if(typeListeners[i] == behavior) {
                            typeListeners.RemoveAt(i);
                        }
                    }

                    if(typeListeners.Count == 0) {
                        _dispatchListeners.Remove(type);
                    }
                }
            }
        }

        public void Dispatch<T>(Action<T> methodToExecute) where T : IDispatchListener {
            Type typeT = typeof(T);
            CleanListenerList(typeT);

            if (_dispatchListeners.ContainsKey(typeT)) {
                foreach(IDispatchListener listener in _dispatchListeners[typeT]) {
                    methodToExecute((T)listener);
                }
            }
        }

        public TDispatchListener Dispatch<TDispatchListener>() where TDispatchListener : IDispatchListener {
            Type listenerType = typeof(TDispatchListener);
            if (!_dispatchMap.ContainsKey(listenerType)) {
                throw new HQException("No listeners registered for type '" + listenerType + "'");
            }

            TDispatchListener listener;
            var collection = _dispatchMap[listenerType];
            if (listenerType.IsAssignableFrom(collection.GetType())) {
                listener = (TDispatchListener)Convert.ChangeType(collection, listenerType);
            }

            throw new HQException("Collection for type '" + listenerType.Name + "' should itself implement " + listenerType.Name);
        }

        public List<T> GetListenersForType<T>() where T : IDispatchListener {
            List<T> list = new List<T>();
            Type typeT = typeof(T);
            if (_dispatchListeners.ContainsKey(typeT)) {
                List<IDispatchListener> listeners = _dispatchListeners[typeT];
                foreach(var listener in listeners) {
                    list.Add((T)listener);
                }
            }


            return list;
        }

        private void CleanListenerList(Type listenerType) {
            if (_dispatchListeners.ContainsKey(listenerType)) {
                List<IDispatchListener> list = _dispatchListeners[listenerType];
                for (int i = list.Count - 1; i >= 0; i--) {
                    if (list[i] == null || !listenerType.IsAssignableFrom(list[i].GetType())) {
                        //This object is no longer relevant in this list.
                        list.RemoveAt(i);
                    }
                }
            }
        }

        private bool DispatchListenerFilter(Type typeObj, Object criteriaObj) {
            bool assignable = typeof(IDispatchListener).IsAssignableFrom(criteriaObj.GetType());
            return assignable;
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