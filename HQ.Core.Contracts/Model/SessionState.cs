using System.Collections.Generic;
//using HQCore.DataSources;
//using HQCore.Models;

//TODO: Need an easy way to map a state to a type of controller
//This project shouldn't really have any reference back to HQ

namespace HQ.Contracts {
    public class SessionState : ControllerState {
        public List<ControllerState> ControllerStates { get; set; }

        public SessionState() : base {
            ControllerStates = new List<ControllerState>();
        }

        public SessionState(List<ControllerState> controllerStates) {
            this.ControllerStates = controllerStates;
        }

        public SessionState(SessionState session) : base{
            this.ControllerStates = session.ControllerStates;
        }
 
        //[DataMember]
        //public AggregateDataSource<TestModelData> testDataSource = new AggregateDataSource<TestModelData>();

        //[DataMember]
        //public Dictionary<Type, HQStateBehavior> Behaviors = new Dictionary<Type, HQStateBehavior>();
        /*[DataMember]
        public Dictionary<string, HQStateBehavior> Controllers = new Dictionary<string, HQStateBehavior>();
        [DataMember]
        public Dictionary<string, HQStateBehavior> Services = new Dictionary<string, HQStateBehavior>();*/

        /*public static BehaviorCategory GetBehaviorCategory(Type behaviorType) {
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

        public Dictionary<string, HQStateBehavior> GetBehaviorsForCategory(BehaviorCategory category) {
            switch (category) {
                case BehaviorCategory.Controller:
                    return Controllers;
                case BehaviorCategory.Service:
                    return Services;
            }
            return new Dictionary<string, HQStateBehavior>();
        }

        public List<Dictionary<string, HQStateBehavior>> GetAllBehaviors() {
            var list = new List<Dictionary<string, HQStateBehavior>>();
            list.Add(Controllers);
            list.Add(Services);
            return list;
        }

        public bool Contains<TType>() {
            Type typeT = typeof(TType);
            BehaviorCategory category = GetBehaviorCategory(typeT);
            return GetBehaviorsForCategory(category).ContainsKey(typeT.Name);
        }

        public void Add<TType>(HQStateBehavior behavior) where TType : HQStateBehavior {
            Type typeT = typeof(TType);
            BehaviorCategory category = GetBehaviorCategory(typeT);
            var behaviors = GetBehaviorsForCategory(category);
            if (!behaviors.ContainsKey(typeT.Name))
                behaviors.Add(typeT.Name, behavior);
        }

        public TType Get<TType>() where TType : HQStateBehavior {
            Type typeT = typeof(TType);
            BehaviorCategory category = GetBehaviorCategory(typeT);
            var behaviors = GetBehaviorsForCategory(category);
            if (behaviors.ContainsKey(typeT.Name))
                return (TType)behaviors[typeT.Name];
            return null;
        }

        public void Remove<TType>() where TType : HQStateBehavior {
            Type typeT = typeof(TType);
            BehaviorCategory category = GetBehaviorCategory(typeT);
            var behaviors = GetBehaviorsForCategory(category);
            if (behaviors.ContainsKey(typeT.Name))
                behaviors.Remove(typeT.Name);
        }*/
    }
}
