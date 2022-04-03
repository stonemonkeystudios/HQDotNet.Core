using System;
using System.IO;
using System.Reflection;
using System.Text;
using HQ.Models;
using HQ.StateMachine.Model;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

/*
 * TODO: Right now we are not explicitly serializing ONLY states
 * We are actually serializing the behavior itself AND the state
 * 
 * This means that anything that has been injected will also serialize somehow?
 * --ooooh because
 * 
 */


namespace HQ {
    /// <summary>
    /// This is for automatic serialization of classes to subtypes.
    /// </summary>
    public static class HQStateMachine{

        private static KnownTypesModel _knownBehaviors;
        public static KnownTypesModel KnownBehaviors {
            get {
                if (_knownBehaviors == null) {
                    _knownBehaviors = GetInternalKnownTypes();
                }
                return _knownBehaviors;
            }
            private set {
                _knownBehaviors = value;
            }
        }

        public static void BuildKnownTypes() {
            _knownBehaviors = null;
            _knownBehaviors = GetInternalKnownTypes();
        }

        public static void AddKnownTypes(KnownTypesModel newModel) {
            foreach(Type t in newModel.types) {
                if (!KnownBehaviors.types.Contains(t)) {
                    _knownBehaviors.types.Add(t);
                }
            }
        }

        public static TStateBehavior DeserializeBehavior<TStateBehavior>(string json) where TStateBehavior : HQStateBehavior, new() {

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var stream = new MemoryStream();
            var ser = new DataContractJsonSerializer(typeof(TStateBehavior), KnownBehaviors.types);
            TStateBehavior deserializedState = (TStateBehavior)ser.ReadObject(ms);
            return deserializedState;
        }

        public static string SerializeState(HQStateBehavior stateBehavior) {
            return SerializeState(stateBehavior.State);
        }

        public static string SerializeState(BaseStateModel stateModel) {
            //throw new NotImplementedException("This currently hits an infinite loop.");
            var stream = new MemoryStream();
            var ser = new DataContractJsonSerializer(stateModel.GetType(), KnownBehaviors.types);
            ser.WriteObject(stream, stateModel);


            stream.Position = 0;
            var sr = new StreamReader(stream);
            string s = sr.ReadToEnd();

            //string s = JsonUtility.ToJson(this);
            return s;
        }

        public static string SerializeBehavior(HQStateBehavior behavior) {
            var stream = new MemoryStream();
            var ser = new DataContractJsonSerializer(behavior.GetType(), KnownBehaviors.types);
            ser.WriteObject(stream, behavior);


            stream.Position = 0;
            var sr = new StreamReader(stream);
            string s = sr.ReadToEnd();

            //string s = JsonUtility.ToJson(this);
            return s;
        }

        public static T DeserializeState<T>(string json) where T : BaseStateModel {
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var stream = new MemoryStream();
            var ser = new DataContractJsonSerializer(typeof(T), KnownBehaviors.types);
            T deserializedState = (T)ser.ReadObject(ms);
            return deserializedState;
        }

        public static BaseStateModel DeserializeState(string json) {
            return DeserializeState<BaseStateModel>(json);
        }

        public static KnownTypesModel GetInternalKnownTypes() {
            Assembly assembly = typeof(BaseStateModel).Assembly;
            string stateModelNamespace = typeof(BaseStateModel).Namespace;
            string behaviorNamespace = typeof(HQStateBehavior).Namespace;
            KnownTypesModel model = GetKnownStateModelTypes(assembly);
            return model;
        }

        public static KnownTypesModel GetKnownStateModelTypes(Assembly assembly) {
            KnownTypesModel model = new KnownTypesModel();
            Type[] assemblyTypes = assembly.GetTypes();
            foreach (Type type in assemblyTypes) {
                /*Type baseType = type;

                //Data sources are not serializable, so for now let's brute force exclude them
                while (baseType.BaseType != null) {
                    if (baseType.Name.ToLower().Contains("datasource"))
                        continue;
                    baseType = baseType.BaseType;
                }*/

                bool addType = false;
                if (typeof(IModelData).IsAssignableFrom(type)) {
                    //This is model data for use in data sources
                    addType = true;
                }
                else if (typeof(BaseStateModel).IsAssignableFrom(type)) {
                    //This is a model for use in states.
                    //This may go away and states serialized as a data contract on the controller itself.
                    //1. State is currently mixing controllers and statesanyway
                    //2. States have already become more than just data, like in HQStateModel
                    //3. It is more readable on a human level and perhaps more easy to integrate with outside systems if the contract is on the controller itself?
                    //Counterpoints?
                    //1. How muddy does this make the difference between data and control?
                    addType = true;
                }
                else if (typeof(HQStateBehavior).IsAssignableFrom(type)) {
                    //Obviously this is any behavior, but relates the the discussion above.
                    addType = true;
                }

                if(addType) {
                    model.types.Add(type);
                }
            }
            return model;
        }
    }
}