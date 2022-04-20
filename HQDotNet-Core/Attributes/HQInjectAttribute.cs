using System;
using System.Reflection;

namespace HQDotNet {

    /// <summary>
    /// [HQInject] is an attribute to be used for private fields on an HQBehavior
    /// It indicates to HQ that we should attempt to inject a registered behavior into the variable
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
        public static void FindInjectableFields(object injectionObject, Action<object, FieldInfo> injectionLogic) {
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