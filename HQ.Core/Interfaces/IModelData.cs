//using HQCore.DataSources;
//using HQCore.Models;
//using Unity.Collections.LowLevel.Unsafe;
//using System.Runtime.Serialization;

namespace HQ { 
    /// <summary>
    /// Base interface for any struct we want to use in a DataSource
    /// </summary>
    public interface IModelData {
        int ID { get; set; }
    }

    public class ValidationModel : IModelData {
        public const int INVALID_ID = 0;
        public int ID { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }

    public static class ModelDataExtensions {
        


        //integer default is 0, so any valid model should have an id > 0
        public static bool IsValid(this IModelData modelData) {
            return modelData.ID != ValidationModel.INVALID_ID;
        }

        /// <summary>
        /// Extension for structs checking UnsafeUtility.IsBlittable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool IsBlittable<T>(this T item) where T : struct {
            return false;// UnsafeUtility.IsBlittable<T>();
        }
    }
}