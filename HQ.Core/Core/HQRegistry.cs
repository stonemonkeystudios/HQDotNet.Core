using System;
using System.Collections.Generic;
using System.Linq;
using HQDotNet.Model;
using HQDotNet.Service;

/*
 * Maps to build
 * 
 * 
 */

namespace HQDotNet {
    public sealed class HQRegistry : HQController<HQRegistryModel> {
        public List<HQCoreBehavior<HQBehaviorModel>> Behaviors { get; set; }
        private HQModelBehaviorBinding ModelToBehaviorBinding { get; set; }

        private HQDispatcherBinding<IDispatchListener> DispatcherBinding { get; set; }

        public void BindBehavior(HQCoreBehavior<HQBehaviorModel> behavior) {
            Type modelType = behavior.Model.GetType();
            Type behaviorType = behavior.GetType();

            var mappedModels = ModelToBehaviorBinding.Where((mappedModel) => { return mappedModel.Value.GetType() == behaviorType});
            if (mappedModels.Count() > 0) {
                throw new HQException("A mapping of type '" + behaviorType + "' already exists.");
            }

            if (!ModelToBehaviorBinding.TryAdd(modelType, behavior)) {
                throw new HQException("A Mapping of Type '" + modelType + "'  already exists.");
            }
        }

        public void BindListener <TListenerBehavior>(TListenerBehavior behavior) where TListenerBehavior : IDispatchListener {
            Type listenerType = typeof(TListenerBehavior);

            if (!DispatcherBinding.ContainsKey(listenerType)) {
                DispatcherBinding.Add(listenerType, new DispatchListenerCollection<IDispatchListener>());
            }

            DispatcherBinding[listenerType].Add(behavior);
        }

        /// <summary>
        /// May not be necessary. This was written to bind any behaviors that were registered
        /// </summary>
        /*public void Remap() {
            ModelToBehaviorBinding.Clear();

            foreach(var behavior in Behaviors) {
                ModelToBehaviorBinding.Add(behavior.Model.GetType(), behavior);
            }
        }*/

        #region Model to Behavior Binding

        public HQController<TBehaviorModel> GetBehaviorForModelType<TBehaviorModel>()
            where TBehaviorModel : HQBehaviorModel, new() {

            Type modelType = typeof(TBehaviorModel);
            if (ModelToBehaviorBinding.ContainsKey(modelType))
                return ModelToBehaviorBinding[modelType] as HQController<TBehaviorModel>;

            return null;
        }

        public List<HQCoreBehavior<TBehaviorModel>> GetBehaviorsForModelType<TBehaviorModel>()
            where TBehaviorModel : HQBehaviorModel, new() {

            List<HQCoreBehavior<TBehaviorModel>> foundBehaviors = new List<HQCoreBehavior<TBehaviorModel>>();
            Behaviors.ForEach((model) => {
                if (typeof(TBehaviorModel).IsAssignableFrom(model.GetType())) {
                    var behavior = GetBehaviorForModelType<TBehaviorModel>();
                    foundBehaviors.Add(behavior);
                }
            });

            return foundBehaviors;
        }

        public static BehaviorCategory GetBehaviorCategory(Type behaviorType) {
            if ((typeof(HQSession)).IsAssignableFrom(behaviorType))
                return BehaviorCategory.HQ;

            if ((typeof(HQController<HQControllerModel>).IsAssignableFrom(behaviorType)))
                return BehaviorCategory.Controller;

            if ((typeof(HQService<HQServiceModel>).IsAssignableFrom(behaviorType)))
                return BehaviorCategory.Service;

            return BehaviorCategory.Invalid;
        }

        #endregion

        #region Dispatcher Bindings

        public IDispatchListener GetDispatchListenerForType<TDispatchListener>()
            where TDispatchListener : IDispatchListener{

            Type listenerType = typeof(TDispatchListener));

            if (DispatcherBinding.ContainsKey(listenerType)) {
                return DispatcherBinding[listenerType];
            }
            return new DispatchListenerCollection<TDispatchListener>();
        }

        #endregion;

        #region Private Binding Classes
        private sealed class HQModelBehaviorBinding : Dictionary<Type, HQCoreBehavior<HQBehaviorModel>> { };

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
