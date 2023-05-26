using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Pong {
    public interface IPongBallPositionUpdatedListener : IDispatchListener {
        void BallPositionUpdated(Vector3 position);
    }
}