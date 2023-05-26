using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HQDotNet.Unity.Pong {
    public interface IGameOverButtonClickedListener : IDispatchListener {
        void OnMainMenuButtonClicked();
        void OnPlayAgainClicked();
    }
}