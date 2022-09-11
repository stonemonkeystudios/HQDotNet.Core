using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Samples {
    /// <summary>
    /// Orchestrating session manager for a basic sphere example
    /// </summary>
    public class BaseLerpSampleSession : HQSessionSetupMonoBehavior {
        public override void Awake() {
            base.Awake();

            //Controller to direct the logic of the scene
            _session.RegisterController<LerpExampleController>();
            
            //Register the base service that will be used to lerp a time
            //This may be switched out for a subclass and will still be injected into the controller.
            _session.RegisterService<BaseLerpService>();
            //_session.RegisterService<SmoothStepLerpService>();

            //Dispatch initial settings to the system
            LerpSettings lerpSettings = new LerpSettings() { lerpTime = 2f, maximumExtent = 2, minimumExtent = -2};
            _session.Dispatcher.Dispatch<IModelListener<LerpSettings>>(listener => listener.OnModelUpdated(lerpSettings));
        }
    }
}
