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
            return _instance;
        }
    }

    public static void CreateInstance() {
        if (Application.isPlaying && _instance == null) {
            GameObject obj = new GameObject("_MainThreadSyncer");
            _instance = obj.AddComponent<MainThreadSyncer>();
        }
    }

    public static void DestroyInstance() {
        Destroy(_instance.gameObject);
        _instance = null;
    }

    public void Awake() {
        _unityMainThread = SynchronizationContext.Current;
        DontDestroyOnLoad(gameObject);
    }

    public void ExecuteOnMainThread(System.Action action) {
        if (_unityMainThread != null) {
            _unityMainThread.Post((_) => action(), null);
        }
    }
}
