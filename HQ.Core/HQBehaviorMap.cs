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
    public sealed class HQBehaviorBindings : HQBehavior<HQBehaviorModel> {
        private class HQMapModelTypeToBehavior : Dictionary<Type, HQBehavior<HQBehaviorModel>> {
            public Type modelType;
            public HQBehavior<HQBehaviorModel> behavior;
        }


        private List<HQBehavior<HQBehaviorModel>> Behaviors { get; set; }
        private HQMapModelTypeToBehavior ModelTypeToBehaviorMap { get; set; }

        public HQBehaviorBindings() {
            Behaviors = new List<HQBehavior<HQBehaviorModel>>();
            ModelTypeToBehaviorMap = new HQMapModelTypeToBehavior();
        }

        public bool MapBehavior(HQBehavior<HQBehaviorModel> behavior) {
            Type modelType = behavior.Model.GetType();
            Type behaviorType = behavior.GetType();

            var mappedModels = ModelTypeToBehaviorMap.Where((mappedModel) => { return mappedModel.Value.GetType() == behaviorType});
            if (mappedModels.Count() > 0) {
                throw new HQException("A mapping of type '" + behaviorType + "' already exists.");
            }

            if (!ModelTypeToBehaviorMap.TryAdd(modelType, behavior)) {
                throw new HQException("A Mapping of Type '" + modelType + "'  already exists.");
            }


            return true;
        }

        public void Remap() {
            ModelTypeToBehaviorMap.Clear();

            foreach(var behavior in Behaviors) {
                ModelTypeToBehaviorMap.Add(behavior.Model.GetType(), behavior);
            }
        }

        public HQBehavior<TBehaviorModel> GetBehaviorForModelType<TBehaviorModel>()
            where TBehaviorModel : HQBehaviorModel, new() {

            Type modelType = typeof(TBehaviorModel);
            if (ModelTypeToBehaviorMap.ContainsKey(modelType))
                return ModelTypeToBehaviorMap[modelType] as HQBehavior<TBehaviorModel>;

            return null;
        }

        public override bool Startup(){
            Remap();
            return base.Startup();
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

    }
}
