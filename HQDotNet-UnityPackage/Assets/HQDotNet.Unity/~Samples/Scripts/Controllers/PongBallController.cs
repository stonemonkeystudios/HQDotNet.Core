using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Pong {
    public class PongBallController : HQController, IModelListener<PongSettings>, IPongStateChangedListener {
        private PongSettings pongSettings;

        private Vector3 currentBallPosition = Vector3.zero;
        private Vector2 travelVector = Vector2.zero;
        private float velocity = 0f;

        [HQInject]
        private PongStateController _stateController;

        public void SetVelocity(float velocity) {
            this.velocity = velocity;
        }

        public void SetTravelVector(Vector2 vector) {
            this.travelVector = vector;
        }

        void IModelListener<PongSettings>.OnModelUpdated(PongSettings model) {
            this.pongSettings = model;
        }

        void IPongStateChangedListener.StateChanged(PongStateController.PongState oldState, PongStateController.PongState newState) {
            if(newState == PongStateController.PongState.Playing) {
                SetVelocity(this.pongSettings.StartingBallSpeed);
                SetTravelVector(new Vector2(-1f, .5f));
            }
        }

        public override void Update() {
            if(_stateController.CurrentState == PongStateController.PongState.Playing) {
                currentBallPosition.x += travelVector.x * velocity * Time.deltaTime;
                currentBallPosition.y += travelVector.y * velocity * Time.deltaTime;
                Session.Dispatcher.Dispatch<IPongBallPositionUpdatedListener>(listener => listener.BallPositionUpdated(currentBallPosition));
            }
            base.Update();
        }
    }
}