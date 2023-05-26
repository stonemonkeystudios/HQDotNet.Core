using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace HQDotNet.Unity.Pong {
    public class PongGameUIView : HQMonoView, IPongStateChangedListener, IPongCountdownUpdatedListener, IPlayerScoredController {
        public Text player1Score;
        public Text player2Score;
        public Text countdown;

        void Start() {
            Assert.IsNotNull(player1Score);
            Assert.IsNotNull(player2Score);
            Assert.IsNotNull(countdown);
        }

        void IPongCountdownUpdatedListener.OnCountdownUpdated(int secondsRemaining) {

            //There is a good chance that the caller of this method is off the main thread, so we must sync back to the main thread
            MainThreadSyncer.Instance.ExecuteOnMainThread(() => {
                countdown.text = secondsRemaining.ToString();
            });
        }

        void IPongStateChangedListener.StateChanged(PongStateController.PongState oldState, PongStateController.PongState newState) {
            //There is a good chance that the caller of this method is off the main thread, so we must sync back to the main thread
            MainThreadSyncer.Instance.ExecuteOnMainThread(() => {
                countdown.gameObject.SetActive(newState == PongStateController.PongState.WaitingToStart);
            });
        }

        void IPlayerScoredController.PlayerScored(int playerIndex, int currentScore) {
            if(playerIndex == 0) {
                player1Score.text = currentScore.ToString();
            }
            else {
                player2Score.text = currentScore.ToString();
            }
        }
    }
}