using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Pong {
    public interface IBallHitPaddleListener : IDispatchListener {
        void OnBallHitPaddle(int playerIndex);
    }
}