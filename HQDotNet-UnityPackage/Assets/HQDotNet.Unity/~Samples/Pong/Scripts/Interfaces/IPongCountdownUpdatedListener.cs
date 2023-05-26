using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HQDotNet.Unity.Pong {
    public interface IPongCountdownUpdatedListener : IDispatchListener {
        void OnCountdownUpdated(int secondsRemaining);
    }
}