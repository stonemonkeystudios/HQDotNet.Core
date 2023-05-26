using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace HQDotNet.Unity.Pong {
    public class PongMainMenuView : HQMonoView, IPongStateChangedListener {
        public Button playButton;
        public GameObject mainMenu;

        private PongStateController _pongStateController;

        void Start() {
            Assert.IsNotNull(playButton);
            Assert.IsNotNull(mainMenu);

            _pongStateController = _session.GetController<PongStateController>();
            playButton.onClick.AddListener(PlayButton_OnClick);
        }

        void PlayButton_OnClick() {
            _pongStateController.SetState(PongStateController.PongState.WaitingToStart);
        }

        void IPongStateChangedListener.StateChanged(PongStateController.PongState oldState, PongStateController.PongState newState) {
            MainThreadSyncer.Instance.ExecuteOnMainThread(() => {
                gameObject.SetActive(newState == PongStateController.PongState.Menu);
            });
        }
    }

}