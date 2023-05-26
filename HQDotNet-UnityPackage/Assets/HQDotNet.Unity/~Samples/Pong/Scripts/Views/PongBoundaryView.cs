using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Pong {
    public class PongBoundaryView : HQMonoView {
        private void OnTriggerEnter(Collider other) {
            if(other.tag == "Ball") {
                _session.Dispatcher.Dispatch<IBallHitBoundaryListener>(listener => listener.OnBallHitBoundary());
            }
        }
    }
}