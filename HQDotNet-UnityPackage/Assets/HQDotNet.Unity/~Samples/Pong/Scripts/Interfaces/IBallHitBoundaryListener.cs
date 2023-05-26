using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Pong {
    public interface IBallHitBoundaryListener : IDispatchListener {
        void OnBallHitBoundary();
    }
}