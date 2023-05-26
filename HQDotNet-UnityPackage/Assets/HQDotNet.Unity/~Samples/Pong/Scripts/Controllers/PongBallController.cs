using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Pong {
    public class PongBallController : HQController, IModelListener<PongSettings>, IPongStateChangedListener, IBallHitPaddleListener, IBallHitBoundaryListener, IGameOverButtonClickedListener {
        public int LastPlayerToScore {get{return lastPlayertoScore; } }
        private PongSettings pongSettings;

        private Vector3 currentBallPosition = Vector3.zero;
        private Vector2 travelVector = Vector2.zero;
        private float velocity = 0f;
        private int lastPlayertoScore = 1;

        [HQInject]
        private PongStateController _stateController;

        public void SetLastScoredPlayer(int lastPlayerToScore) {
            this.lastPlayertoScore = lastPlayerToScore;
        }

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
                SetTravelVector(new Vector2( LastPlayerToScore == 1 ? -1f : 1f, .5f));
            }
            else if(newState == PongStateController.PongState.WaitingToStart) {
                SetVelocity(0f);
                SetTravelVector(Vector2.zero);
                currentBallPosition = Vector3.zero;
                Session.Dispatcher.Dispatch<IPongBallPositionUpdatedListener>(listener => listener.BallPositionUpdated(currentBallPosition));
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

        void IBallHitPaddleListener.OnBallHitPaddle(int playerIndex) {
            travelVector.x = -travelVector.x;
        }

        void IBallHitBoundaryListener.OnBallHitBoundary() {
            travelVector.y = -travelVector.y;
        }

        void IGameOverButtonClickedListener.OnMainMenuButtonClicked() {
            lastPlayertoScore = 1;
        }

        void IGameOverButtonClickedListener.OnPlayAgainClicked() {
            lastPlayertoScore = 1;
        }
    }
}