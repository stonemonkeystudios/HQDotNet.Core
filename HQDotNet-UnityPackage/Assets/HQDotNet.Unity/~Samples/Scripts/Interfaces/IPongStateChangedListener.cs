using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Pong {
    public interface IPongStateChangedListener : IDispatchListener {
        void StateChanged(PongStateController.PongState oldState, PongStateController.PongState newState);
    }
}