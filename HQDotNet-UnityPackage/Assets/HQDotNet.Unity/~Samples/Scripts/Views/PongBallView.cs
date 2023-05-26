using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Pong {
    public class PongBallView : HQMonoView, IPongBallPositionUpdatedListener {
        void IPongBallPositionUpdatedListener.BallPositionUpdated(Vector3 position) {
            MainThreadSyncer.Instance.ExecuteOnMainThread(() => {
                transform.position = position;
            });
        }
    }

}