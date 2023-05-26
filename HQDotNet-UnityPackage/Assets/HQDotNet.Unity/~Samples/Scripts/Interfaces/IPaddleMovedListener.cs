using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Pong {
    public interface IPaddleMovedListener : IDispatchListener {
        void PaddleMoved(int playerIndex, float paddleYPosition);
    }
}