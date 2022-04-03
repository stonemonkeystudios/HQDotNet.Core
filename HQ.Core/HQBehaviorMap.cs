using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using HQ.Contracts;
using HQ;
using HQ.Controllers;

namespace HQ {
    class HQBehaviorMap : HQStateBehavior<BaseStateModel> {
        public List<HQStateBehavior<BaseStateModel>> Behaviors { get; private set; }

        private Dictionary<Type, Type> _stateToControllerMap;

        public HQBehaviorMap() {
            Behaviors = new List<HQStateBehavior<BaseStateModel>>();
            _stateToControllerMap = new Dictionary<Type, Type>();
        }

        public bool MapBehavior(HQStateBehavior<BaseStateModel> behavior) {
            Type behaviorType = behavior.GetType();
            Type stateType = behavior.State.GetType();

        }

        public void Remap() {
            _stateToControllerMap = new Dictionary<Type, Type>();

            foreach(var behavior in Behaviors) {
                Type behaviorType = behavior.GetType();
                BehaviorCategory category = GetBehaviorCategory(behaviorType);
                switch (category) {
                    case BehaviorCategory.Controller:
                        _stateToControllerMap.Add(behaviorType, behavior.State.GetType());
                        break;
                }
            }
        }

        public override bool Startup(){
            Remap();
            return base.Startup();
        }

        public static BehaviorCategory GetBehaviorCategory(Type behaviorType) {
            if ((typeof(HQSession)).IsAssignableFrom(behaviorType))
                return BehaviorCategory.HQ;

            if ((typeof(Controller).IsAssignableFrom(behaviorType)))
                return BehaviorCategory.Controller;

            if ((typeof(HQService).IsAssignableFrom(behaviorType)))
                return BehaviorCategory.Service;

            bool isDataSource = false;
            if (behaviorType.ContainsGenericParameters) {
                foreach (Type t in behaviorType.GenericTypeArguments) {
                    isDataSource |= typeof(IModelData).IsAssignableFrom(t);
                }
            }
            if (isDataSource)
                return BehaviorCategory.DataSource;

            return BehaviorCategory.Invalid;
        }

    }
}
