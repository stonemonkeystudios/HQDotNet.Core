using System.Collections.Generic;
using HQDotNet
//using HQCore.DataSources;
//using HQCore.Models;

//TODO: Need an easy way to map a state to a type of controller
//This project shouldn't really have any reference back to HQ

namespace HQDotNet.Model {
    public class HQSessionModel : HQControllerModel {
        public HQProjectModel Project { get; set; }
        public List<HQControllerModel> ControllerModels { get; set; }
        public List<HQViewModel> ViewModels { get; set; }
        public List<HQServiceModel> ServiceModels { get; set; }

        public HQSessionModel() : base {
            ControllerModels = new List<HQControllerModel>();
        }

        public HQSessionModel(List<HQControllerModel> controllerStates) {
            this.ControllerModels = controllerStates;
        }

        public HQSessionModel(HQSessionModel session) : base{
            this.ControllerModels = session.ControllerModels;
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
