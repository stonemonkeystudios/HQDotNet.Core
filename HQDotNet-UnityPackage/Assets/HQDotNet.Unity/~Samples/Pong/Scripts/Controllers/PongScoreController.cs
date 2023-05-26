using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Pong {
    public class PongScoreController : HQController, IModelListener<PongSettings>, IGameOverButtonClickedListener {

        [HQInject]
        private PongStateController _stateController;

        [HQInject]
        private PongBallController _ballController;

        private PongSettings _settings;

        public int Player1Score { get; private set; }
        public int Player2Score { get; private set; }

        public void ScoreForPlayer(int playerIndex) {
            if(playerIndex == 0) {
                Player1Score++;
            }
            else {
                Player2Score++;
            }
            _ballController.SetLastScoredPlayer(playerIndex);
            //Dispatch score update
            Session.Dispatcher.Dispatch<IPlayerScoredController>(listener => listener.PlayerScored(playerIndex, playerIndex == 0 ? Player1Score : Player2Score));
            CheckEndGame();
        }

        private void CheckEndGame() {
            if(Player1Score >= _settings.WinningScore || Player2Score >= _settings.WinningScore) {
                _stateController.SetState(PongStateController.PongState.GameOver);
            }
            else {
                _stateController.SetState(PongStateController.PongState.WaitingToStart);
            }
        }

        void IGameOverButtonClickedListener.OnMainMenuButtonClicked() {
            ClearScore();
        }
        void IGameOverButtonClickedListener.OnPlayAgainClicked() {
            ClearScore();
        }

        void ClearScore() {
            Player1Score = 0;
            Player2Score = 0;
            Session.Dispatcher.Dispatch<IPlayerScoredController>(listener => listener.PlayerScored(0, Player1Score));
            Session.Dispatcher.Dispatch<IPlayerScoredController>(listener => listener.PlayerScored(1, Player2Score));
        }

        void IModelListener<PongSettings>.OnModelUpdated(PongSettings model) {
            this._settings = model;
        }

    }
}