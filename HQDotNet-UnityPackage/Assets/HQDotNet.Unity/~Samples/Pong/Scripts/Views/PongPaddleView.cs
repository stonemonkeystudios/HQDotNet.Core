using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Pong {
    public class PongPaddleView : HQMonoView, IPaddleMovedListener {
        public int playerIndex;

        void OnTriggerEnter(Collider other) {
            if(other.tag == "Ball") {
                _session.Dispatcher.Dispatch<IBallHitPaddleListener>(listener => listener.OnBallHitPaddle(playerIndex));

                //TODO: Check what % up the paddle the hit is and vary the changed direction based on that
            }
        }


        void IPaddleMovedListener.PaddleMoved(int playerIndex, float paddleYPosition) {
            if (this.playerIndex == playerIndex) {
                MainThreadSyncer.Instance.ExecuteOnMainThread(() => {
                    Vector3 pos = transform.position;
                    pos.y = paddleYPosition;
                    transform.position = pos;
                });
            }
        }
    }
}