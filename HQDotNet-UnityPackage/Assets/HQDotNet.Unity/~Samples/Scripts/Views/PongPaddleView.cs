using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Pong {
    public class PongPaddleView : HQMonoView, IPaddleMovedListener {
        public int playerIndex;
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