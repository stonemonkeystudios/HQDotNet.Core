using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace HQDotNet.Unity.Pong {
    public class PongStateController : HQController, IGameOverButtonClickedListener {
        public enum PongState { Menu, WaitingToStart, Playing, GameOver, None };

        public PongState CurrentState { get { return currentState; } }

        private PongState currentState = PongState.None;

        public override bool Startup() {

            //Start the game on the main menu
            SetState(PongState.Menu);
            return base.Startup();
        }

        public void SetState(PongState newState) {
            var oldState = currentState;
            currentState = newState;
            Session.Dispatcher.Dispatch<IPongStateChangedListener>(listener => listener.StateChanged(oldState, currentState));
            Task.Run(CheckState);
        }

        private async Task CheckState() {
            if(currentState == PongState.WaitingToStart) {
                await Task.Run(Countdown);
                SetState(PongState.Playing);
            }
        }

        private async Task Countdown() {
            for(int i = 3; i > 0; i--) {
                if(currentState == PongState.WaitingToStart) {
                    Session.Dispatcher.Dispatch<IPongCountdownUpdatedListener>(listener => listener.OnCountdownUpdated(i));
                    await Task.Delay(System.TimeSpan.FromSeconds(1));
                }
            }
        }

        void IGameOverButtonClickedListener.OnMainMenuButtonClicked() {
            SetState(PongState.Menu);
        }

        void IGameOverButtonClickedListener.OnPlayAgainClicked() {
            SetState(PongState.WaitingToStart);
        }
    }
}