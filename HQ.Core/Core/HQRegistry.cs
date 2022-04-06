using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HQ.Model;
using HQ;
using HQ.Controller;
using HQ.Service;

/*
 * Maps to build
 * 
 * 
 */

namespace HQ {
    public sealed class HQRegistry : HQSingletonBehavior<HQBehaviorModel> {
        public List<HQSingletonBehavior<HQBehaviorModel>> Behaviors { get; set; }
        private HQModelBehaviorBinding ModelToBehaviorBinding { get; set; }

        private HQDispatcherBinding DispatcherBinding { get; set; }

        public void BindBehavior(HQSingletonBehavior<HQBehaviorModel> behavior) {
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

        public void Remap() {
            ModelToBehaviorBinding.Clear();

            foreach(var behavior in Behaviors) {
                ModelToBehaviorBinding.Add(behavior.Model.GetType(), behavior);
            }
        }

        public HQSingletonBehavior<TBehaviorModel> GetBehaviorForModelType<TBehaviorModel>()
            where TBehaviorModel : HQBehaviorModel, new() {

            Type modelType = typeof(TBehaviorModel);
            if (ModelToBehaviorBinding.ContainsKey(modelType))
                return ModelToBehaviorBinding[modelType] as HQSingletonBehavior<TBehaviorModel>;

            return null;
        }

        public override bool Startup(){
            Remap();
            return base.Startup();
        }

        public List<HQSingletonBehavior<TBehaviorModel>> GetBehaviorsForModelType<TBehaviorModel>()
            where TBehaviorModel : HQBehaviorModel, new() {

            List<HQSingletonBehavior<TBehaviorModel>> foundBehaviors = new List<HQSingletonBehavior<TBehaviorModel>>();
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

            if ((typeof(HQService).IsAssignableFrom(behaviorType)))
                return BehaviorCategory.Service;

            return BehaviorCategory.Invalid;
        }



        #region Private Binding Classes
        private class HQModelBehaviorBinding : Dictionary<Type, HQSingletonBehavior<HQBehaviorModel>> { };

        private class HQDispatcherBinding : Dictionary<Type, DispatchListenerCollection<IDispatchListener>> { }

        #endregion

    }
}
