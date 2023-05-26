using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HQDotNet.Unity;

namespace HQDotNet.Unity.Pong {
    public class PongSession : HQSessionSetupMonoBehavior {
        public PongSettings pongSettings;

        public override void Awake() {
            base.Awake();
            DontDestroyOnLoad(gameObject);

            //Register Controllers
            _session.RegisterController<PongBallController>();
            _session.RegisterController<PongStateController>();

            //Register Services

            //Send Model Data
            _session.Dispatcher.Dispatch<IModelListener<PongSettings>>(listener => listener.OnModelUpdated(pongSettings));
        }
    }

}