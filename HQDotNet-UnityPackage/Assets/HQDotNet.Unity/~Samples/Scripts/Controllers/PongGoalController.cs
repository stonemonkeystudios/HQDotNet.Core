using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Pong {
    public class PongGoalController : HQController, IBallHitPlayerGoalListener {
        [HQInject]
        PongScoreController scoreController;

        public void BallHitPlayerGoal(int playerNumber) {
            scoreController.ScoreForPlayer(playerNumber);
        }
    }
}