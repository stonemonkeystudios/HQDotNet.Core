using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace HQDotNet.Unity.Pong {
    public class PongGameOverView : HQMonoView, IPongStateChangedListener {
        public Button mainMenuButton;
        public Button playAgainButton;

        private void Start() {
            Assert.IsNotNull(mainMenuButton);
            Assert.IsNotNull(playAgainButton);
            mainMenuButton.onClick.AddListener(MainMenuButton_OnClick);
            playAgainButton.onClick.AddListener(PlayAgainButton_OnClick);
        }

        private void MainMenuButton_OnClick() {
            _session.Dispatcher.Dispatch<IGameOverButtonClickedListener>(listener => listener.OnMainMenuButtonClicked());
        }

        private void PlayAgainButton_OnClick() {
            _session.Dispatcher.Dispatch<IGameOverButtonClickedListener>(listener => listener.OnPlayAgainClicked());
        }

        void IPongStateChangedListener.StateChanged(PongStateController.PongState oldState, PongStateController.PongState newState) {
            MainThreadSyncer.Instance.ExecuteOnMainThread(() => {
                gameObject.SetActive(newState == PongStateController.PongState.GameOver);
            });
        }
    }
}