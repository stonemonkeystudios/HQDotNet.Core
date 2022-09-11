using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Samples {

    /// <summary>
    /// This is a very basic service that will transform elapsed time into a bounded lerp time (0-1)
    /// Then, using settings given by an IModelListener, it will transform that value into a transmittable value
    /// From there, the rest of the app will be notified via dispatch that this value has been updated
    /// </summary>
    public class BaseLerpService : HQService, IModelListener<LerpSettings> {

        /// <summary>
        /// Settings to transform our normalized looping time value
        /// </summary>
        protected LerpSettings _settings;

        /// <summary>
        /// Transform elapsed time into a normalized looping value, transform it acording to settings
        /// </summary>
        /// <param name="elapsed">Time since the HQSession was started</param>
        public virtual void PerformLerp(float elapsed) {
            float t = (elapsed % _settings.lerpTime) / _settings.lerpTime;
            t = Mathf.Lerp(_settings.minimumExtent, _settings.maximumExtent, t);
            Session.Dispatcher.Dispatch<ILerpListener>(listener => listener.Lerp(t));
        }

        /// <summary>
        /// A LerpSettings model has been updated, respond accordingly
        /// </summary>
        /// <param name="model"></param>
        void IModelListener<LerpSettings>.OnModelUpdated(LerpSettings model) {
            _settings = model;
        }
    }
}