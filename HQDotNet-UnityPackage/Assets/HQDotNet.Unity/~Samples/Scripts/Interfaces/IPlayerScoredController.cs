using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Pong {
    public interface IPlayerScoredController : IDispatchListener {
        void PlayerScored(int playerIndex, int currentScore);
    }
}