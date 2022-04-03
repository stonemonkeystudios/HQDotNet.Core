using System;
using System.Reflection;

namespace HQ {

    /// <summary>
    /// [HQInject] is an attribute that can be used on private fields to attempt to inject various types of HQ objects into
    /// Currently supported objects are DataSource<IDataStore> and Controller.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class HQInject : Attribute {
        public HQInject() {}

        /// <summary>
        /// Analyzes the given object to find a private field with an [HQInject] attribute.
        /// If one is found, it is given to the injectionLogic delegate for processing.
        /// </summary>
        /// <param name="injectionObject">The current object we are searching for fields to inject.</param>
        /// <param name="injectionLogic">The logic to be executed if a match is found.</param>
        public static void FindInjectableFields(System.Object injectionObject, Action<System.Object, FieldInfo> injectionLogic) {
            if(injectionLogic == null) {
                throw new ArgumentException("Injection logic null.");
            }

            Type type = injectionObject.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields) {
                HQInject inject = field.GetCustomAttribute<HQInject>(true);
                if (inject != null) {
                    injectionLogic(injectionObject, field);

                }
            }
        }
    }
}