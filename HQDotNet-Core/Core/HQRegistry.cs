using System;
using System.Collections.Generic;
using System.Linq;
using HQDotNet.Model;
using HQDotNet.View;
using HQDotNet.Service;

/*
 * Maps to build
 * 
 * 
 */

namespace HQDotNet {
    internal sealed class HQRegistry : HQController<HQRegistryModel> {
        //public List<HQObject> Behaviors { get; private set; }

        public Dictionary<Type, HQController<HQControllerModel>> Controllers { get; private set; }
        public Dictionary<Type, HQService<HQServiceModel>> Services { get; private set; }
        public Dictionary<Type, List<HQView<HQDataModel, HQViewModel<HQDataModel>>>> Views { get; private set; }

        private Dictionary<Type, DispatchListenerCollection<IDispatchListener>> DispatcherBinding { get; set; }

        public HQRegistry() {
            Controllers = new Dictionary<Type, HQController<HQControllerModel>>();
            Services = new Dictionary<Type, HQService<HQServiceModel>>();
            Views = new Dictionary<Type, List<HQView<HQDataModel, HQViewModel<HQDataModel>>>>();

        }

        /// <summary>
        /// Register a singleton behavior for a type of controller 
        /// </summary>
        /// <param name="controller"></param>
        public void RegisterController<TBehavior, TModel>(TBehavior controller)
            where TBehavior : HQController<HQControllerModel>, new()
            where TModel : HQControllerModel, new() {

            if (Controllers.ContainsKey(controller.GetType()))
                throw new HQException("Behavior type is already registered.");
            Controllers.Add(controller.Model.GetType(),controller);
        }

        /// <summary>
        /// Register a singleton behavior for a type of service 
        /// </summary>
        /// <param name="controller"></param>
        public void RegisterService<TBehavior, TModel>(TBehavior service)
            where TBehavior : HQService<HQServiceModel>, new()
            where TModel : HQServiceModel, new() {

            if (Services.ContainsKey(service.GetType()))
                throw new HQException("Behavior type is already registered.");
            Services.Add(service.Model.GetType(), service);
        }

        public void RegisterView<TBehavior, TModel>(TBehavior view) 
            where TBehavior : HQView<HQDataModel, HQViewModel<HQDataModel>>, new()
            where TModel : HQViewModel<HQDataModel>, new(){

            if (!Views.ContainsKey(view.GetType()))
                Views.Add(view.Model.GetType(), new List<HQView<HQDataModel, HQViewModel<HQDataModel>>>());
            Views[view.Model.GetType()].Add(view);
        }

        public void BindListener <TListenerBehavior>(TListenerBehavior behavior) where TListenerBehavior : IDispatchListener {
            Type listenerType = typeof(TListenerBehavior);

            if (!DispatcherBinding.ContainsKey(listenerType)) {
                DispatcherBinding.Add(listenerType, new DispatchListenerCollection<IDispatchListener>());
            }

            DispatcherBinding[listenerType].Add(behavior);
        }

        public static BehaviorCategory GetBehaviorCategory(Type behaviorType) {
            if ((typeof(HQSession)).IsAssignableFrom(behaviorType))
                return BehaviorCategory.HQ;

            if ((typeof(HQController<HQControllerModel>).IsAssignableFrom(behaviorType)))
                return BehaviorCategory.Controller;

            if ((typeof(HQService<HQServiceModel>).IsAssignableFrom(behaviorType)))
                return BehaviorCategory.Service;

            if (typeof(HQView<HQDataModel, HQViewModel<HQDataModel>>).IsAssignableFrom(behaviorType)) {
                return BehaviorCategory.View;
            }

            return BehaviorCategory.Invalid;
        }

        #region Dispatcher Bindings

        public IDispatchListener GetDispatchListenerForType<TDispatchListener>()
            where TDispatchListener : IDispatchListener{

            Type listenerType = typeof(TDispatchListener);

            if (DispatcherBinding.ContainsKey(listenerType)) {
                return DispatcherBinding[listenerType];
            }
            return new DispatchListenerCollection<TDispatchListener>();
        }

        #endregion;

        #region Private Binding Classes
        private sealed class HQDispatcherBinding<TDispatcherListener> : Dictionary<Type, TDispatcherListener> where TDispatcherListener : IDispatchListener { }

        #endregion

        #region HQBehavior Overrides

        public override bool Startup() {
            //Remap any behaviors that were bound during HQSession initialization
            //Remap();
            return base.Startup();
        }


        #endregion
    }
}
