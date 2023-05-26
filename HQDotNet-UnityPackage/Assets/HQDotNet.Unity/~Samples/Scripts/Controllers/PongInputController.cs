using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Pong {
    public class PongInputController : HQController {
        [HQInject]
        PongPaddleController _paddleController;

        public override void Update() {
            base.Update();

            if (Input.GetKey(KeyCode.W)) {
                //Player 1 up
                _paddleController.MovePaddle(0, 1);
            }
            else if (Input.GetKey(KeyCode.S)) {
                //Player 1 down
                _paddleController.MovePaddle(0, -1);
            }

            if (Input.GetKey(KeyCode.O)) {
                //Player 2 Up
                _paddleController.MovePaddle(1, 1);
            }
            else if (Input.GetKey(KeyCode.L)) {
                //Player 2 Down
                _paddleController.MovePaddle(1, -1);
            }
        }
    }
}