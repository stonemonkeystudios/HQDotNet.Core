using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Samples {

    /// <summary>
    /// The same as <see cref="BaseLerpService"/>, but does a SmoothStep calculation rather than a simple Lerp
    /// </summary>
    public class SmoothStepLerpService : BaseLerpService{

        /// <summary>
        /// Smoothly interpolate between a minimum and maximum extent
        /// </summary>
        /// <param name="elapsed">Time since the HQSession was started</param>
        public override void PerformLerp(float elapsed) {
            float t = (elapsed % _settings.lerpTime) / _settings.lerpTime;
            t = Mathf.SmoothStep(_settings.minimumExtent, _settings.maximumExtent, t);
            Session.Dispatcher.Dispatch<ILerpListener>(listener => listener.Lerp(t));
        }
    }
}