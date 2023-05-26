using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Pong {
    public class PongGoalView : HQMonoView {
        public int playerNumber = 0;

        private void OnTriggerEnter(Collider other) {
            if(other.tag == "Ball") {
                Debug.Log("Ball hit goal " + playerNumber);
                _session.Dispatcher.Dispatch<IBallHitPlayerGoalListener>(listener => listener.BallHitPlayerGoal(playerNumber));
            }
        }
    }
}