using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Samples {

    /// <summary>
    /// Controls the flow of logic throughout the example.
    /// </summary>
    public class LerpExampleController : HQController{

        /// <summary>
        /// This may be a BaseLerpService or any subclass therein, as long as they are uniquely injected
        /// </summary>
        [HQInject]
        BaseLerpService _lerpService;

        /// <summary>
        /// Every update cycle, give out injected service an input time and let it process the request
        /// </summary>
        public override void Update() {
            ///Elapsed time since the session started
            float time = (float)((System.DateTime.Now - Session.StartDate).TotalMilliseconds) / 1000f;
            _lerpService.PerformLerp(time);
            base.Update();
        }
    }
}
