using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Samples {
    /// <summary>
    /// Interface for any class that wants to listen for time alterations from this example
    /// </summary>
    public interface ILerpListener : IDispatchListener {
        void Lerp(float lerpValue);
    }
}