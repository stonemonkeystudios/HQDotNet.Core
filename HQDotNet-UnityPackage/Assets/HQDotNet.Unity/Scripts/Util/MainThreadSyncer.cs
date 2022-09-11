using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MainThreadSyncer : MonoBehaviour
{
    private SynchronizationContext _unityMainThread;

    private static MainThreadSyncer _instance;
    public static MainThreadSyncer Instance {
        get {
            if(_instance == null) {
                GameObject obj = new GameObject();
                _instance = obj.AddComponent<MainThreadSyncer>();
            }
            return _instance;
        }
    }

    public void Awake() {
        _unityMainThread = SynchronizationContext.Current;
    }

    public void ExecuteOnMainThread(System.Action action) {
        _unityMainThread.Post((_)=>action(), null);
    }
}
