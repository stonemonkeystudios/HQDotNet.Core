using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Pong {
    public class PongPaddleController : HQController, IModelListener<PongSettings> {
        private float paddle1Position;
        private float paddle2Position;
        private PongSettings settings;

        public void MovePaddle(int playerIndex, int yDir) {
            if (playerIndex == 0) {
                paddle1Position += Time.deltaTime * yDir * settings.PaddleSpeed;
                paddle1Position = Mathf.Clamp(paddle1Position, -settings.MaxPaddleYPosition, settings.MaxPaddleYPosition);
                Session.Dispatcher.Dispatch<IPaddleMovedListener>(listener => listener.PaddleMoved(playerIndex, paddle1Position));
            }
            else {
                paddle2Position += Time.deltaTime * yDir * settings.PaddleSpeed;
                paddle2Position = Mathf.Clamp(paddle2Position, -settings.MaxPaddleYPosition, settings.MaxPaddleYPosition);
                Session.Dispatcher.Dispatch<IPaddleMovedListener>(listener => listener.PaddleMoved(playerIndex, paddle2Position));
            }
        }

        void IModelListener<PongSettings>.OnModelUpdated(PongSettings model) {
            this.settings = model;
        }
    }
}